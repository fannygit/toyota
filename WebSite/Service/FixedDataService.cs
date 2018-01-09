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
using JamZoo.Project.WebSite.Enums;
using Begonia.Toyota.WebSite.Library;

namespace Begonia.Toyota.WebSite.Service
{
    public class FixedDataService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public FixedDataModel NewInstance()
        {
            FixedDataModel newOne = new FixedDataModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public bool UpdateALL(string create_id)
        {
            var all = from p in basedb.fixed_data2
                      select p;

            foreach (var i in all)
            {
                i.create_id = create_id;
            }
            basedb.SaveChanges();
            return true;

        }

        public bool Create(string UserName, FixedDataModel model, out string ErrMsgs)
        {
            model.CreateId = UserName;
            model.CreateTime = DateTime.Now;

            ErrMsgs = string.Empty;


            fixed_data dbEntity = new fixed_data();
            dbEntity.Id = Utils.GetObjectId();
            dbEntity.type = model.FunctionName;
            dbEntity.html = model.Html;
            dbEntity.create_time = model.CreateTime;
            dbEntity.create_id = model.CreateId;
            basedb.fixed_data.Add(dbEntity);
            

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

        public bool Create2(string UserName, FixedDataModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;


            fixed_data2 dbEntity = new fixed_data2();
            dbEntity.Id = Utils.GetObjectId();
            dbEntity.type = model.FunctionName;
            dbEntity.create_date = DateTime.Now;
            dbEntity.create_id = UserName;

            if (model.FunctionName.Equals(FixedData2.現有職缺設定.ToString()))
            {
                dbEntity.c2 = model.HumanBankUrl;
            }
            else
            {
                dbEntity.c1 = model.MailBoxSender;
                dbEntity.c2 = model.MailBoxReceiver;
            }

            basedb.fixed_data2.Add(dbEntity);


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

        public FixedDataModel Get(string userId, string fn)
        {
            var o_entity = (from p in basedb.fixed_data
                            where p.type.Equals(fn)
                            select p).FirstOrDefault();
            if (o_entity!=null)
            {
                FixedDataModel model = new FixedDataModel(); 
                model.Id = o_entity.Id;
                model.FunctionName = o_entity.type;
                model.Html = o_entity.html;
                model.CreateTime = o_entity.create_time;
                model.CreateId = o_entity.create_id;
                return model;
            }
            else
            {
                return null;
            }            
        }

        public FixedDataModel Get2(string userId, string fn)
        {
            var o_entity = (from p in basedb.fixed_data2
                            where p.type.Equals(fn)
                            select p).FirstOrDefault();
            
            if (o_entity!=null)
            {

                FixedDataModel model = new FixedDataModel();
                model.Id = o_entity.Id;
                model.FunctionName = o_entity.type;
                model.CreateTime = o_entity.create_date;
                model.CreateId = o_entity.create_id;

                if (o_entity.type.Equals(FixedData2.現有職缺設定.ToString()))
                {
                    model.HumanBankUrl = o_entity.c2;
                }
                else
                {
                    model.MailBoxSender = o_entity.c1;
                    model.MailBoxReceiver = o_entity.c2;
                }


                return model;
            }
            else
            {
                return null;
            }
        }

        public bool Update(string userId, string userAccount, FixedDataModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;
            fixed_data o_entity = new fixed_data();

            try
            {
            
                    o_entity.Id = model.Id;
                    o_entity.type = model.FunctionName;
                    o_entity.html = model.Html;
                    o_entity.create_time = model.CreateTime;
                    o_entity.create_id = model.CreateId; 
                    
                    basedb.fixed_data.Attach(o_entity);
                    basedb.Entry(o_entity).Property(x => x.create_time).IsModified = true;
                    basedb.Entry(o_entity).Property(x => x.create_id).IsModified = true;

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


        public bool Update2(string userId, string userAccount, FixedDataModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;
            fixed_data2 o_entity = new fixed_data2();

            try
            {

                o_entity.Id = model.Id;
                o_entity.type = model.FunctionName;
                o_entity.create_date = model.CreateTime;
                o_entity.create_id = model.CreateId;

                if (model.FunctionName.Equals(FixedData2.現有職缺設定.ToString()))
                {
                    o_entity.c2 = model.HumanBankUrl;
                }
                else
                {
                    o_entity.c1 = model.MailBoxSender;
                    o_entity.c2 = model.MailBoxReceiver;
                }

                basedb.fixed_data2.Attach(o_entity);
                basedb.Entry(o_entity).Property(x => x.create_date).IsModified = true;
                basedb.Entry(o_entity).Property(x => x.create_id).IsModified = true;

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

        
   }
}