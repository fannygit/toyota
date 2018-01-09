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

namespace Begonia.Toyota.WebSite.Service
{
    public class ProductAccessoriesService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public ProductAccessoriesModel NewInstance()
        {
            ProductAccessoriesModel newOne = new ProductAccessoriesModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public ProductAccessoriesListModel GetList1(int pid, ProductAccessoriesListModel Page)
        {
            var o_query = from p in basedb.product_accessories
                          join i in basedb.product_info_accessories_list on p.Id equals i.accessories_id
                          where i.product_id == pid
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.type.Contains(Page.Search));
            }


            switch (Page.SStatus)
            {
                case 0:
                    o_query = o_query.Where(p => !p.status);
                    break;
                case 1:
                    o_query = o_query.Where(p => p.status);
                    break;
            }

            switch (Page.OrderBy)
            {
                case 1:
                    o_query = o_query.OrderBy(p => p.name_red);
                    Page.OrderBy = 2;
                    break;
                case 2:
                    o_query = o_query.OrderByDescending(p => p.name_red);
                    Page.OrderBy = 1;
                    break;
                default:
                    Page.OrderBy++;
                    o_query = o_query.OrderBy(p => p.orderfield).ThenByDescending(p => p.create_time);
                    break;
            }

            var query = o_query;

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new ProductAccessoriesModel()
                    {
                        Id = o_entity.Id,
                        NameR = o_entity.name_red,
                        NameB = o_entity.name_black,
                        Type = o_entity.type,
                        Img = o_entity.img,
                        Detail = o_entity.detail,
                        Orderby = o_entity.orderfield,
                        Status = o_entity.status,
                        CreateTime = o_entity.create_time,
                        CreateId = o_entity.create_id,

                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }


        public ProductAccessoriesListModel GetList(string CurrentUserName, ProductAccessoriesListModel Page)
        {
           var o_query = from p in basedb.product_accessories
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.type.Contains(Page.Search));
            }


            switch (Page.SStatus)
            {
                case 0:
                    o_query = o_query.Where(p => !p.status);
                    break;
                case 1:
                    o_query = o_query.Where(p => p.status);
                    break;
            }

            switch (Page.OrderBy)
            {
                case 1:
                    o_query = o_query.OrderBy(p => p.name_red);
                    Page.OrderBy = 2;
                    break;
                case 2:
                    o_query = o_query.OrderByDescending(p => p.name_red);
                    Page.OrderBy = 1;
                    break;
                default:
                    Page.OrderBy++;
                    o_query = o_query.OrderBy(p => p.orderfield).ThenByDescending(p=>p.create_time);
                    break;
            }

            var query = o_query;

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new ProductAccessoriesModel()
                    {
						Id = o_entity.Id,
						NameR = o_entity.name_red,
                        NameB = o_entity.name_black,
						Type = o_entity.type,
						Img = o_entity.img,
						Detail = o_entity.detail,
                        Orderby = o_entity.orderfield,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,

                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        

        public bool Create(string UserName, ProductAccessoriesModel model, out string ErrMsgs)
        {
            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<ProductAccessoriesModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Img", model.ImgFile);
            }

            ErrMsgs = string.Empty;

            product_accessories dbEntity = new product_accessories();
			dbEntity.Id = model.Id;
            dbEntity.name_red = model.NameR;
            dbEntity.name_black = model.NameB;
			dbEntity.type = model.Type;
			dbEntity.img = model.Img;
			dbEntity.detail = model.Detail;
            dbEntity.orderfield = model.Orderby;
			dbEntity.status = model.Status;
			dbEntity.create_time = model.CreateTime;
			dbEntity.create_id = UserName;


            basedb.product_accessories.Add(dbEntity);

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

		public ProductAccessoriesModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.product_accessories
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new ProductAccessoriesModel()
                    {
						Id = o_entity.Id,
						NameR = o_entity.name_red,
                        NameB = o_entity.name_black,
						Type = o_entity.type,
						Img = o_entity.img,
						Detail = o_entity.detail,
                        Orderby = o_entity.orderfield,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,

                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, ProductAccessoriesModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<ProductAccessoriesModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Img", model.ImgFile);
            }

            try
            {
                product_accessories o_entity = new product_accessories()
                {
					Id = model.Id,
					name_red = model.NameR,
                    name_black = model.NameB,
					type = model.Type,
					img = model.Img,
					detail = model.Detail,
                    orderfield = model.Orderby,
					status = model.Status,
					create_time = model.CreateTime,
					create_id = model.CreateId,

                };
				
                basedb.product_accessories.Attach(o_entity);
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

        public bool Delete(string userName, int? id, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            var o_delete = from p in basedb.product_accessories
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.product_accessories.Remove(row);
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