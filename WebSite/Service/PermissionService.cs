/*
ModelName
dbModelName
KeyFieldType
KeyEexpression
DaoKeyFieldName
UpdateFields
Mapping_Fields_A
Mapping_Fields_B
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data;
using System.Data.Objects;

using Begonia.Toyota.WebSite.Enums;
using Begonia.Toyota.WebSite.Models;

using Begonia.Toyota.WebSite.DbContext;
using Begonia.Toyota.WebSite.Library;

namespace Begonia.Toyota.WebSite.Service
{
    public class PermissionService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public PermissionModel NewInstance()
        {
            PermissionModel newOne = new PermissionModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        /// <summary>
        /// 初始化後台權限(清空再加入預設)
        /// </summary>
        public void InitPermission(out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                //foreach (var mapping in basedb.T_Permission)
                //{
                //    basedb.T_Permission.Remove(mapping);
                //}

                //basedb.SaveChanges();


                foreach (var i in Enum.GetValues(typeof(Role)).Cast<Role>())
                {
                    
                        T_Permission permission = new T_Permission();
                        permission.Id = i.ToString();
                        permission.CreateDate = DateTime.Now;
                        permission.Name = i.ToString();
                        permission.CreateUserId = INIT_ACCOUNT;
                        permission.UpdateDate = DateTime.Now;
                        permission.UpdateUserId = INIT_ACCOUNT;
                        basedb.T_Permission.Add(permission);
                    
                }

                basedb.SaveChanges();
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
        }
        
        public PermissionListModel GetList(string CurrentUserName, PermissionListModel Page)
        {
           var o_query = from p in basedb.T_Permission
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
            }

            var query = o_query.OrderBy(p => p.Id);

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new PermissionModel()
                    {
						Id = o_entity.Id,
						Name = o_entity.Name,
						Createdate = o_entity.CreateDate,
						Updatedate = o_entity.UpdateDate,
						Createuserid = o_entity.CreateUserId,
						Updateuserid = o_entity.UpdateUserId,

                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }      

        public bool Create(string UserName, PermissionModel model, out string ErrMsgs)
        {


            ErrMsgs = string.Empty;

            T_Permission dbEntity = new T_Permission();
			dbEntity.Id = model.Id;
			dbEntity.Name = model.Name;
			dbEntity.CreateDate = model.Createdate;
			dbEntity.UpdateDate = model.Updatedate;
			dbEntity.CreateUserId = model.Createuserid;
			dbEntity.UpdateUserId = model.Updateuserid;

            basedb.T_Permission.Add(dbEntity);

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

		public PermissionModel Get(string userId, string id)
        {
            var o_entity = (from p in basedb.T_Permission
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new PermissionModel()
                    {
						Id = o_entity.Id,
						Name = o_entity.Name,
						Createdate = o_entity.CreateDate,
						Updatedate = o_entity.UpdateDate,
						Createuserid = o_entity.CreateUserId,
						Updateuserid = o_entity.UpdateUserId,

                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, PermissionModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;



            try
            {
                T_Permission o_entity = new T_Permission()
                {
					Id = model.Id,
					Name = model.Name,
					CreateDate = model.Createdate,
					UpdateDate = DateTime.Now,
					CreateUserId = model.Createuserid,
					UpdateUserId = model.Updateuserid,

                };
				
                basedb.T_Permission.Attach(o_entity);
				basedb.Entry(o_entity).Property(x => x.CreateDate).IsModified = true;

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

            var o_delete = from p in basedb.T_Permission
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.T_Permission.Remove(row);
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

        
   }
}