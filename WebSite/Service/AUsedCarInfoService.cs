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
using System.Web.Mvc;
using Begonia.Toyota.WebSite.Enums;
using Begonia.Toyota.WebSite.Models;
using Begonia.Toyota.WebSite.DbContext;

namespace Begonia.Toyota.WebSite.Service
{
    public class AUsedCarInfoService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public AUsedCarInfoModel NewInstance()
        {
            AUsedCarInfoModel newOne = new AUsedCarInfoModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public AUsedCarInfoListModel GetList1(string CurrentUserName, AUsedCarInfoListModel Page, bool show = false)
        {
            var o_query = from p in basedb.a_used_car_info
                          select p;

            if (show)
            {
                o_query = o_query.Where(p=>p.status==true);
            }

            if (!string.IsNullOrEmpty(Page.type) && Page.type != "所有產車")
            {
                o_query = o_query.Where(p => p.product_type_id == Page.type);
            }
            if (!string.IsNullOrEmpty(Page.eng) && Page.eng != "所有引擎")
            {
                o_query = o_query.Where(p => p.engine_id == Page.eng);
            }
            if (!string.IsNullOrEmpty(Page.ton) && Page.ton != "所有噸數")
            {
                o_query = o_query.Where(p => p.tonnes == Page.ton);
            }
            if (!string.IsNullOrEmpty(Page.yy) && Page.yy != "所有年份")
            {
                o_query = o_query.Where(p => p.years == Page.yy);
            }

            var query = o_query;

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Select(o_entity =>
                    new AUsedCarInfoModel()
                    {
                        Id = o_entity.Id,
                        Type = o_entity.type,
                        ProductType = o_entity.product_type_id,
                        ListImg = o_entity.list_img,
                        Number = o_entity.number,
                        EngineId = (from eg in basedb.a_used_car_engine
                                      where eg.Id == o_entity.engine_id
                                      select eg.type).FirstOrDefault(),
                        Tool = o_entity.tool,
                        Tonnes = o_entity.tonnes,
                        Length = o_entity.length,
                        EngineNumber = o_entity.engine_number,
                        CarNumber = o_entity.car_number,
                        Years = o_entity.years,
                        UsedHours = o_entity.used_hours,
                        Price = o_entity.price,
                        ContactPerson = o_entity.contact_person,
                        ContactMethod = o_entity.contact_method,
                        Orderby = o_entity.orderfield == 0 ? o_entity.orderfield + 99 : o_entity.orderfield,
                        Status = o_entity.status,
                        CreateTime = o_entity.create_time,
                        CreateId = o_entity.create_id,
                        ProductImg1 = (from img in basedb.a_used_car_info_img
                                      where img.a_used_car_info_id == o_entity.Id
                                      orderby img.orderfield
                                      select img.img).FirstOrDefault()
                    }
                    ).ToList();

                Page.Data = Page.Data.OrderBy(p => p.Orderby).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }


        public List<string> GetImgList(int? id)
        {
            List<string> arr = new List<string>();

            arr = (from p in basedb.a_used_car_info_img
                   where p.a_used_car_info_id == id
                   orderby p.orderfield
                   select p.img).ToList();

            return arr;
        }

        public AUsedCarInfoModel Get1(int? id)
        {
            var o_entity = (from p in basedb.a_used_car_info
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new AUsedCarInfoModel()
                {
                    Id = o_entity.Id,
                    Type = o_entity.type,
                    ProductType = o_entity.product_type_id.ToString(),
                    ListImg = o_entity.list_img,
                    Number = o_entity.number,
                    EngineId = (from eg in basedb.a_used_car_engine
                                where eg.Id == o_entity.engine_id
                                select eg.type).FirstOrDefault(),
                    Tool = o_entity.tool,
                    Tonnes = o_entity.tonnes,
                    Length = o_entity.length,
                    EngineNumber = o_entity.engine_number,
                    CarNumber = o_entity.car_number,
                    Years = o_entity.years,
                    UsedHours = o_entity.used_hours,
                    Price = o_entity.price,
                    ContactPerson = o_entity.contact_person,
                    ContactMethod = o_entity.contact_method,
                    Orderby = o_entity.orderfield,
                    Status = o_entity.status,
                    CreateTime = o_entity.create_time,
                    CreateId = o_entity.create_id,
                };
                return model;
            }
            return null;
        }

        public List<SelectListItem> GetEngineList(List<string> idList)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var o_query = from p in basedb.a_used_car_engine
                          where idList.Contains(p.type)
                          select p;
            foreach (var v in o_query)
            {
                list.Add(new SelectListItem
                {
                    Text = v.type.ToString(),
                    Value = v.Id.ToString()
                });
            }
            return list;
        }

        public List<SelectListItem> GetEngineList(string eid)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var o_query = from p in basedb.a_used_car_engine
                          select p;
            foreach (var v in o_query)
            {
                list.Add(new SelectListItem
                {
                    Text = v.type.ToString(),
                    Value = v.Id.ToString(),
                    Selected = (eid == v.Id.ToString())
                });
            }
            return list;
        }

        public AUsedCarInfoListModel GetList(string CurrentUserName, AUsedCarInfoListModel Page)
        {
           var o_query = from p in basedb.a_used_car_info
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                //產品機型/編號
                o_query = o_query.Where(p => p.type.Contains(Page.Search) || p.number.Contains(Page.Search));
            }

            if (!string.IsNullOrEmpty(Page.ProductType) && Page.ProductType != "4")
            {
                //產品機型/編號
                o_query = o_query.Where(p => p.product_type_id == Page.ProductType);
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
                    if (Page.OrderField == "t")
                    {
                        o_query = o_query.OrderBy(p => p.type);
                    }
                    else if (Page.OrderField == "p")
                    {
                        o_query = o_query.OrderBy(p => p.product_type_id);
                    }
                    else if (Page.OrderField == "n")
                    {
                        o_query = o_query.OrderBy(p => p.number);
                    }
                    Page.OrderBy = 2;
                    break;
                case 2:
                    if (Page.OrderField == "t")
                    {
                        o_query = o_query.OrderByDescending(p => p.type);
                    }
                    else if (Page.OrderField == "p")
                    {
                        o_query = o_query.OrderByDescending(p => p.product_type_id);
                    }
                    else if (Page.OrderField == "n")
                    {
                        o_query = o_query.OrderByDescending(p => p.number);
                    }
                    Page.OrderBy = 1;
                    break;
                default:
                    o_query = o_query.OrderByDescending(p => p.create_time);
                    break;
            }

            var query = o_query;

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new AUsedCarInfoModel()
                    {
						Id = o_entity.Id,
						Type = o_entity.type,
                        ProductType = o_entity.product_type_id,
						ListImg = o_entity.list_img,
						Number = o_entity.number,
                        EngineId = o_entity.engine_id,
						Tool = o_entity.tool,
						Tonnes = o_entity.tonnes,
						Length = o_entity.length,
						EngineNumber = o_entity.engine_number,
						CarNumber = o_entity.car_number,
						Years = o_entity.years,
						UsedHours = o_entity.used_hours,
						Price = o_entity.price,
						ContactPerson = o_entity.contact_person,
						ContactMethod = o_entity.contact_method,
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

        public bool Create(string UserName, AUsedCarInfoModel model, out string ErrMsgs)
        {
            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ListImg", model.ImgFile);
            }

            ErrMsgs = string.Empty;

            a_used_car_info dbEntity = new a_used_car_info();
			dbEntity.Id = model.Id;
			dbEntity.type = model.Type;
            dbEntity.product_type_id = model.ProductType;
			dbEntity.list_img = model.ListImg;
			dbEntity.number = model.Number;
			dbEntity.engine_id = model.EngineId;
			dbEntity.tool = model.Tool;
			dbEntity.tonnes = model.Tonnes;
			dbEntity.length = model.Length;
			dbEntity.engine_number = model.EngineNumber;
			dbEntity.car_number = model.CarNumber;
			dbEntity.years = model.Years;
			dbEntity.used_hours = model.UsedHours;
			dbEntity.price = model.Price;
			dbEntity.contact_person = model.ContactPerson;
			dbEntity.contact_method = model.ContactMethod;
            dbEntity.orderfield = model.Orderby;
			dbEntity.status = model.Status;
			dbEntity.create_time = DateTime.Now;
			dbEntity.create_id = UserName;


            basedb.a_used_car_info.Add(dbEntity);

            try
            {

                basedb.SaveChanges();

                List<string> listimg = new List<string>();

                if (model.ImgFile1 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg1", model.ImgFile1);
                    listimg.Add(model.ProductImg1);
                }
                if (model.ImgFile2 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg2", model.ImgFile2);
                    listimg.Add(model.ProductImg2);
                }
                if (model.ImgFile3 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg3", model.ImgFile3);
                    listimg.Add(model.ProductImg3);
                }
                if (model.ImgFile4 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg4", model.ImgFile4);
                    listimg.Add(model.ProductImg4);
                } 
                if (model.ImgFile5 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg5", model.ImgFile5);
                    listimg.Add(model.ProductImg5);
                }
                if (model.ImgFile6 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg6", model.ImgFile6);
                    listimg.Add(model.ProductImg6);
                }

                if (model.ImgFile7 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg7", model.ImgFile7);
                    listimg.Add(model.ProductImg7);
                }

                if (model.ImgFile8 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg8", model.ImgFile8);
                    listimg.Add(model.ProductImg8);
                }

                if (model.ImgFile9 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg9", model.ImgFile9);
                    listimg.Add(model.ProductImg9);
                }

                if (model.ImgFile10 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg10", model.ImgFile10);
                    listimg.Add(model.ProductImg10);
                } 

                int index=0;
                foreach(var i in listimg)
                {
                    index++;
                    a_used_car_info_img dbEntityImg = new a_used_car_info_img();
                    dbEntityImg.a_used_car_info_id = dbEntity.Id;
                    dbEntityImg.img = i;
                    dbEntityImg.orderfield = index;
                    basedb.a_used_car_info_img.Add(dbEntityImg);
                }

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

		public AUsedCarInfoModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.a_used_car_info
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new AUsedCarInfoModel()
                    {
						Id = o_entity.Id,
						Type = o_entity.type,
                        ProductType = o_entity.product_type_id.ToString(),
						ListImg = o_entity.list_img,
						Number = o_entity.number,
                        EngineId = o_entity.engine_id.ToString(),
						Tool = o_entity.tool,
						Tonnes = o_entity.tonnes,
						Length = o_entity.length,
						EngineNumber = o_entity.engine_number,
						CarNumber = o_entity.car_number,
						Years = o_entity.years,
						UsedHours = o_entity.used_hours,
						Price = o_entity.price,
						ContactPerson = o_entity.contact_person,
						ContactMethod = o_entity.contact_method,
                        Orderby = o_entity.orderfield,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,
                    };

                List<string> arr = new List<string>();

                arr = (from p in basedb.a_used_car_info_img
                        where p.a_used_car_info_id == model.Id
                        orderby p.orderfield
                        select p.img).ToList();

                model.ProductImg1 = arr.Any() ? arr[0] : string.Empty;
                model.ProductImg2 = arr.Count() >= 2 ? arr[1] : string.Empty;
                model.ProductImg3 = arr.Count() >= 3 ? arr[2] : string.Empty;
                model.ProductImg4 = arr.Count() >= 4 ? arr[3] : string.Empty;
                model.ProductImg5 = arr.Count() >= 5 ? arr[4] : string.Empty;
                model.ProductImg6 = arr.Count() >= 6 ? arr[5] : string.Empty;
                model.ProductImg7 = arr.Count() >= 7 ? arr[6] : string.Empty;
                model.ProductImg8 = arr.Count() >= 8 ? arr[7] : string.Empty;
                model.ProductImg9 = arr.Count() >= 9 ? arr[8] : string.Empty;
                model.ProductImg10 = arr.Count() >= 10 ? arr[9] : string.Empty;

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, AUsedCarInfoModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ListImg", model.ImgFile);
            }

            try
            {
                a_used_car_info o_entity = new a_used_car_info()
                {
					Id = model.Id,
					type = model.Type,
                    product_type_id = model.ProductType,
					list_img = model.ListImg,
					number = model.Number,
					engine_id = model.EngineId,
					tool = model.Tool,
					tonnes = model.Tonnes,
					length = model.Length,
					engine_number = model.EngineNumber,
					car_number = model.CarNumber,
					years = model.Years,
					used_hours = model.UsedHours,
					price = model.Price,
					contact_person = model.ContactPerson,
					contact_method = model.ContactMethod,
                    orderfield = model.Orderby,
					status = model.Status,
                    create_time = model.CreateTime,
					create_id = model.CreateId,

                };
				
                basedb.a_used_car_info.Attach(o_entity);
				basedb.Entry(o_entity).Property(x => x.create_time).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.create_id).IsModified = true;

                basedb.Entry(o_entity).State = EntityState.Modified;
                basedb.SaveChanges();
                

                #region 全刪
                var allDel = from p in basedb.a_used_car_info_img
                            where p.a_used_car_info_id == model.Id
                            orderby p.orderfield
                            select p;
                foreach (var row in allDel)
                {
                    basedb.a_used_car_info_img.Remove(row);
                }
                basedb.SaveChanges();

                #endregion

                #region 全新增

                if (model.ImgFile1 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg1", model.ImgFile1);
                }

                if (model.ImgFile2 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg2", model.ImgFile2);
                }

                if (model.ImgFile3 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model,HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg3", model.ImgFile3);
                }

                if (model.ImgFile4 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg4", model.ImgFile4);
                }

                if (model.ImgFile5 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg5", model.ImgFile5);
                }

                if (model.ImgFile6 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg6", model.ImgFile6);
                }

                if (model.ImgFile7 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg7", model.ImgFile7);
                }

                if (model.ImgFile8 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg8", model.ImgFile8);
                }

                if (model.ImgFile9 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg9", model.ImgFile9);
                }

                if (model.ImgFile10 != null)
                {
                    Library.Utils.SaveFile<AUsedCarInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg10", model.ImgFile10);
                }

                List<string> addImg = new List<string>();
                if (!string.IsNullOrEmpty(model.ProductImg1)) addImg.Add(model.ProductImg1);
                if (!string.IsNullOrEmpty(model.ProductImg2)) addImg.Add(model.ProductImg2);
                if (!string.IsNullOrEmpty(model.ProductImg3)) addImg.Add(model.ProductImg3);
                if (!string.IsNullOrEmpty(model.ProductImg4)) addImg.Add(model.ProductImg4);
                if (!string.IsNullOrEmpty(model.ProductImg5)) addImg.Add(model.ProductImg5);
                if (!string.IsNullOrEmpty(model.ProductImg6)) addImg.Add(model.ProductImg6);
                if (!string.IsNullOrEmpty(model.ProductImg7)) addImg.Add(model.ProductImg7);
                if (!string.IsNullOrEmpty(model.ProductImg8)) addImg.Add(model.ProductImg8);
                if (!string.IsNullOrEmpty(model.ProductImg9)) addImg.Add(model.ProductImg9);
                if (!string.IsNullOrEmpty(model.ProductImg10)) addImg.Add(model.ProductImg10);

                int index = 1;
                foreach (var i in addImg)
                {
                    a_used_car_info_img dbEntityImg = new a_used_car_info_img();
                    dbEntityImg.a_used_car_info_id = model.Id;
                    dbEntityImg.img = i;
                    dbEntityImg.orderfield = index;
                    basedb.a_used_car_info_img.Add(dbEntityImg);
                    index++;
                }

                basedb.SaveChanges();
                #endregion
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

        [HttpPost]
        public bool Delete(string userName, int? id, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            var o_delete = from p in basedb.a_used_car_info
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.a_used_car_info.Remove(row);
            }

            try
            {
                basedb.SaveChanges();

                var o_deleteimg = from p in basedb.a_used_car_info_img
                                  where p.a_used_car_info_id == id
                                  select p;
                foreach (var row in o_deleteimg)
                {
                    basedb.a_used_car_info_img.Remove(row);
                }
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