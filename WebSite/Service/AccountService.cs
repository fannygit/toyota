using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data;

using System.Web.Mvc;


using Begonia.Toyota.WebSite.Enums;
using Begonia.Toyota.WebSite.Models;
using Begonia.Toyota.WebSite.DbContext;
using Begonia.Toyota.WebSite.Library;
using System.Data.Entity;
using System.Net;


namespace Begonia.Toyota.WebSite.Service
{
    public class AccountService : BaseService
    {

        public void CreateLog(string UserName, string userID, bool login, bool success, string failReson)
        {
            log_login l = new log_login();
            l.CreateTime = DateTime.Now;
            l.AccountId = userID;
            if (string.IsNullOrEmpty(UserName))
            {
                var a = Get(UserName, userID);
                if (string.IsNullOrEmpty(a.Email))
                {
                    l.AccountName = a.Account;
                }
                else
                {
                    l.AccountName = a.Account + "/" + a.Email;
                }
            }
            else
            {
                l.AccountName = UserName;
            }
            
            if (!string.IsNullOrEmpty(failReson))
                l.FailReson = failReson;

            l.IP = GetClientIP();
            l.LoginStatus = success;

            if (login)
            {
                l.LoginTime = DateTime.Now;
            }
            else
            {
                l.LogoutTime = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(userID))
            {
                basedb.log_login.Add(l);
                basedb.SaveChanges();
            }
        }


        public string GetClientIP()
        {
            // 取得本機名稱
            string strHostName = Dns.GetHostName();
            // 取得本機的IpHostEntry類別實體，用這個會提示已過時
            //IPHostEntry iphostentry = Dns.GetHostByName(strHostName);

            // 取得本機的IpHostEntry類別實體，MSDN建議新的用法
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

            // 取得所有 IP 位址
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                // 只取得IP V4的Address
                if (ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ipaddress.ToString();
                }
            }
            return iphostentry.AddressList.Any() ? iphostentry.AddressList.LastOrDefault().ToString() : string.Empty;
        }



        public List<string> GetHaveRoles(string account)
        {
            var havelist = from p in basedb.T_Permission
                           join m in basedb.T_Account_Permission_Mapping on p.Id equals m.PermissionId
                           where m.AccountId == account
                           select p.Name;
            return havelist.ToList();
        }

        public bool Isright(string account, String[] roleList)
        {
            var havelist = from p in basedb.T_Permission
                           join m in basedb.T_Account_Permission_Mapping on p.Id equals m.PermissionId
                           where m.AccountId == account
                           select p.Name;

            foreach (string i in havelist)//循環比對角色資料
            {
                if (roleList.Contains(i))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 取得帳號列表
        /// </summary>
        /// <param name="CurrentUserName">目前使用者ID</param>
        /// <param name="Page">分頁</param>
        /// <returns></returns>
        public AccountListModel GetList(string CurrentUserName, AccountListModel Page)
        {
            var o_query = from p in basedb.T_Account
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.Account.Contains(Page.Search));
            }

            switch (Page.SStatus)
            {
                case 0:
                    o_query = o_query.Where(p => p.Status==0);
                    break;
                case 1:
                    o_query = o_query.Where(p => p.Status==1);
                    break;
            }
            switch (Page.OrderBy)
            {
                case 1:
                    if (Page.OrderField == "an")
                    {
                        o_query = o_query.OrderBy(p => p.Account);
                    }
                    else if (Page.OrderField == "rn")
                    {
                        o_query = o_query.OrderBy(p => p.Role);
                    }
                    else if (Page.OrderField == "e")
                    {
                        o_query = o_query.OrderBy(p => p.Email);
                    }
                    else
                    {
                        o_query = o_query.OrderByDescending(p => p.CreateDate);
                    }
                    Page.OrderBy = 2;
                    break;
                case 2:
                    if (Page.OrderField == "an")
                    {
                        o_query = o_query.OrderByDescending(p => p.Account);
                    }
                    else if (Page.OrderField == "rn")
                    {
                        o_query = o_query.OrderByDescending(p => p.Role);
                    }
                    else if (Page.OrderField == "e")
                    {
                        o_query = o_query.OrderByDescending(p => p.Email);
                    }
                    else
                    {
                        o_query = o_query.OrderByDescending(p => p.CreateDate);
                    }
                    Page.OrderBy = 1;
                    break;
                default:
                    o_query = o_query.OrderByDescending(p => p.CreateDate);
                    break;
            }

            var query = o_query;

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(p =>
                    new AccountModel()
                    {
                        Id = p.Id,
                        Account = p.Account,
                        CreateDate = p.CreateDate,
                        LastLoginTime = p.LastLoginTime,
                        Status = p.Status,
                        Role = p.Role,
                        Email = p.Email
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        /// <summary>
        /// 驗證
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <param name="User"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Authentication(
            string Account,
            string Password,
            out AccountModel User,
            out string errorMsg
            )
        {
            errorMsg = string.Empty;
            string accountId = string.Empty;
            bool isSuccess = Validate(Account, Password, out accountId, out errorMsg);

            User = Get("system", accountId); 

            return errorMsg.Length == 0;
        }

        bool DataValidate(AccountModel model, out string message)
        {
            message = string.Empty;

            if (basedb.T_Account.Where(p => p.Account == model.Account).Select(p => p.Id).Count() > 0)
            {
                message = "此帳號已存在";
            }

            return message.Length == 0;
        }

        /// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public AccountModel NewInstance()
        {
            AccountModel newOne = new AccountModel();
            newOne.Id = Library.Utils.GetObjectId();
            newOne.CreateDate = DateTime.Now;
            newOne.Status = 1;
            return newOne;
        }

        public bool Create(string UserName, AccountModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            //1.Validate
            if (DataValidate(model, out ErrMsgs))
            {
                T_Account dbEntity = new T_Account();
                dbEntity.Id = model.Id;
                dbEntity.Account = model.Account;

                //實作 Hash256 驗證
                dbEntity.Password = model.Password.ToHash256();

                //預設啟用
                dbEntity.Status = model.Status;
                dbEntity.IP = model.IP;
                dbEntity.LastLoginTime = DateTime.Now;
                dbEntity.CreateDate = DateTime.Now;
                dbEntity.UpdateDate = DateTime.Now;
                dbEntity.CreateUserID = model.Account;
                dbEntity.UpdateUserID = model.Account;
                dbEntity.Role = model.Role;
                dbEntity.Email = model.Email;

                if (model.PermissionList.Count() != 0)
                {
                    CreatePermissionList(UserName, dbEntity.Id, model.PermissionList, out ErrMsgs);
                }

                basedb.T_Account.Add(dbEntity);

                try
                {

                    basedb.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        ErrMsgs = ex.InnerException.Message;
                    }
                    else
                    {
                        ErrMsgs = ex.Message;
                    }
                }
            }

            return ErrMsgs.Length == 0;
        }


        public bool CreatePermissionList(string UserName, string Id, List<string> PermissionMappingList, out string ErrMsgs)
        {


            ErrMsgs = string.Empty;
            foreach (var pid in PermissionMappingList)
            {
                T_Account_Permission_Mapping dbEntity = new T_Account_Permission_Mapping();
                dbEntity.Id = Utils.GetObjectId();
                dbEntity.PermissionId = pid;
                dbEntity.CreateDate = DateTime.Now;
                dbEntity.AccountId = Id;
                dbEntity.CreateUserId = UserName;
                
                basedb.T_Account_Permission_Mapping.Add(dbEntity);
            }

            try
            {

                basedb.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ErrMsgs = ex.InnerException.Message;
                }
                else
                {
                    ErrMsgs = ex.Message;
                }
            }

            return ErrMsgs.Length == 0;
        }

        public AccountModel GetByAccount(string Account)
        {
            var o_entity = (from p in basedb.T_Account
                            where p.Account == Account
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new AccountModel()
                {
                    Id = o_entity.Id,
                    Account = o_entity.Account,
                    CreateDate = o_entity.CreateDate,
                    LastLoginTime = o_entity.LastLoginTime,
                    Status = o_entity.Status,
                    Role = o_entity.Role,
                    Email = o_entity.Email
                };
                
                return model;
            }
            return null;
        }

        public AccountModel Get(string userId, string id)
        {
            var o_entity = (from p in basedb.T_Account
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new AccountModel()
                    {
                        Id = o_entity.Id,
                        Account = o_entity.Account,
                        CreateDate = o_entity.CreateDate,
                        LastLoginTime = o_entity.LastLoginTime,
                        Status = o_entity.Status,
                        IP = o_entity.IP,
                        Name = o_entity.Name,
                        Role = o_entity.Role,
                        Email = o_entity.Email,
                        CreateUserID = o_entity.CreateUserID,
                        UpdateUserID = o_entity.UpdateUserID,
                        Password = o_entity.Password,
                        UpdateDate = o_entity.UpdateDate,
                    };

                model.PermissionDropDownList = GetPermission(model.Id);
                return model;
            }
            return null;
        }

        public bool Update(string userName, AccountModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            try
            {
                T_Account o_entity = new T_Account()
                {
                    Id = model.Id,
                    Account = model.Account,
                    CreateDate = model.CreateDate,
                    Status = model.Status,
                    Role = model.Role,
                    Email = model.Email,
                    UpdateDate = model.UpdatePasswordTime,
                    CreateUserID = model.CreateUserID,
                    UpdateUserID = model.Account,
                    Password = model.Password
                };

                if (string.IsNullOrEmpty(model.RePassword))
                {
                    o_entity.Password = model.Password;
                }
                else
                {
                    o_entity.Password = model.RePassword.ToHash256();
                }
                
                basedb.T_Account.Attach(o_entity);
                basedb.Entry(o_entity).Property(x => x.CreateDate).IsModified = true;

                ClearPermissions(model.Id);
                AddUserToPermissions(userName, model.Id, model.PermissionList);

                basedb.Entry(o_entity).State = EntityState.Modified;
                basedb.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ErrMsgs = ex.InnerException.Message;
                }
                else
                {
                    ErrMsgs = ex.Message;
                }
            }

            return ErrMsgs.Length == 0;            
        }

        public bool Delete(string userName, string id, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            var o_delete = from p in basedb.T_Account
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.T_Account.Remove(row);
            }

            try
            {
                basedb.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }

            return ErrorMsg.Length == 0;
        }

        #region 其他函式

        /// <summary>
        /// 初始化最高管理者
        /// </summary>
        /// <param name="data"></param>
        /// <param name="errorMsg"></param>
        public AccountModel InitAdmin(out string errorMsg)
        {
            AccountModel model = NewInstance();
            model.Id = INIT_ACCOUNT;
            model.Account = INIT_ACCOUNT;
            model.Password = "zxcv1234";
            model.Status = 1;
            model.Name = INIT_ACCOUNT;
            model.Role = "Admin";
            model.IP = "";

            model.PermissionList = new List<string>() { "Admin" };
            Create(INIT_ACCOUNT, model, out errorMsg);
          
            return model;
        }

        /// <summary>
        /// 基本驗證
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <param name="AccountId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool Validate(
            string Account,
            string Password,
            out string AccountId,
            out string ErrorMsg
            )
        {
            ErrorMsg = string.Empty;
            AccountId = string.Empty;

            var dbUser = (from p in basedb.T_Account
                          where p.Account == Account
                          select p).FirstOrDefault();

            if (dbUser == null)
            {
                ErrorMsg = "無此帳號";
            }
            else
            {
                string Hash256Pwd = Password.ToHash256();

                if (dbUser.Password != Hash256Pwd)
                {
                    ErrorMsg = "密碼錯誤";
                    CreateLog(Account, dbUser.Id, true, false, ErrorMsg);
                }

                else if (dbUser.Status != 1)
                {
                    ErrorMsg = "帳號停用中,請洽管理員";
                    CreateLog(Account, dbUser.Id, true, false, ErrorMsg);
                }
                else
                {
                    try
                    {
                        AccountId = dbUser.Id;
                        dbUser.LastLoginTime = DateTime.Now;
                        basedb.SaveChanges();
                    }catch(Exception e)
                    {
                        ErrorMsg = "帳號發生異常,請洽管理員";
                    }
                }
            }

            return ErrorMsg.Length == 0;
        }

        /// <summary>
        /// 清除所有角色
        /// </summary>
        /// <param name="AccountId"></param>
        public void ClearPermissions(string AccountId)
        {
            var accountRoleMappings = from p in basedb.T_Account_Permission_Mapping
                                    where p.AccountId == AccountId
                                    select p;

            foreach (var mapping in accountRoleMappings)
            {
                basedb.T_Account_Permission_Mapping.Remove(mapping);
            }
        }


        /// <summary>
        /// 將使用者加到角色
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="RoleNameList"></param>
        /// <returns></returns>
        public bool AddUserToPermissions(string nowLoginUser, string AccountId, List<string> permissionIdList)
        {
            foreach (string pid in permissionIdList)
            {
                T_Account_Permission_Mapping mapping = new T_Account_Permission_Mapping();
                mapping.Id = Utils.GetObjectId();
                mapping.AccountId = AccountId;
                mapping.PermissionId = pid;
                mapping.CreateDate = DateTime.Now;
                mapping.CreateUserId = nowLoginUser;
                basedb.T_Account_Permission_Mapping.Add(mapping);
                basedb.SaveChanges();
            }

            return true;
        }

        
        /// <summary>
        /// 取得權限列表
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetPermission(string accountId)
        {
            List<SelectListItem> permissionList = new List<SelectListItem>();

            var plist = from p in basedb.T_Permission
                        where p.Name != "Admin"
                        select p;

            var haves = from p in basedb.T_Account_Permission_Mapping
                        where p.AccountId == accountId
                        select p;
          

            foreach(var p in plist)
            {
                permissionList.Add(new SelectListItem() { Value = p.Id, Text = p.Name, Selected = (haves.Where(j=>j.PermissionId== p.Id).Any()) });
            }

            return permissionList;
        }
        #endregion
    }
}