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
using System.Web.Mvc;
using Begonia.Toyota.WebSite.Library;

namespace Begonia.Toyota.WebSite.Service
{
    public class ProductInfoService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public ProductInfoModel NewInstance()
        {
            ProductInfoModel newOne = new ProductInfoModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public List<string> GetList1(string key)
        {
            var o_query = from p in basedb.product_info
                          where p.product_type_id == key
                          select p;
            return o_query.Select(p => p.name).ToList();
        }


        public ProductInfoListModel GetList1(ProductInfoListModel Page, bool show = false)
        {
            var o_query = from p in basedb.product_info
                          select p;
            if (show)
            {
                o_query = o_query.Where(p => p.status == true);
            }

            var query = o_query.OrderBy(p=>p.orderfield);

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Select(o_entity =>
                    new ProductInfoModel()
                    {
                        Id = o_entity.Id,
                        Name = o_entity.name,
                        SubTitle = o_entity.sub_title,
                        ProductType = o_entity.product_type_id,
                        EngineId = o_entity.engine_id,
                        Img = o_entity.img,
                        Banner = o_entity.banner,
                        OverviewType = o_entity.overview_type,
                        OverviewTonnesStart = o_entity.overview_tonnes_start,
                        OverviewTonnesEnd = o_entity.overview_tonnes_end,
                        OverviewImg = o_entity.overview_img,
                        OverviewImgIntro = o_entity.overview_img_intro,
                        Introduction = o_entity.introduction,
                        DetailGas = o_entity.detail_gas,
                        DetailDiesel = o_entity.detail_diesel,
                        DetailElectric = o_entity.detail_electric,
                        DetailOverlookImg = o_entity.detail_overlook_img,
                        //DetailSlideImg = o_entity.detail_slide_img,
                        DetailEngineTable = o_entity.detail_engine_table,
                        DetailMotorBatteryTable = o_entity.detail_motor_battery_table,
                        MetaKeywords = o_entity.meta_keywords,
                        MetaDescription = o_entity.meta_description,
                        CreateTime = o_entity.create_time,
                        CreateId = o_entity.create_id,
                        OnShelfDate = o_entity.on_shelf_date,
                        Orderby = o_entity.orderfield == 0 ? o_entity.orderfield + 99 : o_entity.orderfield 
                    }
                    ).ToList();

                Page.Data = Page.Data.OrderBy(p => p.Orderby).ToList();

                foreach(var i in Page.Data)
                {
                    i.OverviewTonnesStartSHOW = (i.OverviewTonnesStart != 0) ? ( i.OverviewTonnesStart % 1 == 0 ? i.OverviewTonnesStart.ToString(".0#") : i.OverviewTonnesStart.ToString() ): string.Empty;
                    i.OverviewTonnesEndSHOW = (i.OverviewTonnesEnd != 0) ? (i.OverviewTonnesEnd % 1 == 0 ? i.OverviewTonnesEnd.ToString(".0#") : i.OverviewTonnesEnd.ToString()): string.Empty;
                }

                string pd1value1 = (Convert.ToInt32(ProductType.堆高機)).ToString();
                string ed1value1 = (Convert.ToInt32(ProductEngineType.內燃)).ToString();
                string ed1value2 = (Convert.ToInt32(ProductEngineType.電動)).ToString();

                Page.Data1 = Page.Data.Where(p => p.ProductType == pd1value1 && p.EngineId == ed1value1).OrderBy(p=>p.Orderby).ToList();
                Page.Data2 = Page.Data.Where(p => p.ProductType == pd1value1 && p.EngineId == ed1value2).OrderBy(p => p.Orderby).ToList();

                string pd1value2 = (Convert.ToInt32(ProductType.鏟裝機)).ToString();
                Page.Data3 = Page.Data.Where(p => p.ProductType == pd1value2 && p.EngineId == ed1value1).OrderBy(p => p.Orderby).ToList();
                Page.Data4 = Page.Data.Where(p => p.ProductType == pd1value2 && p.EngineId == ed1value2).OrderBy(p => p.Orderby).ToList();

                string pd1value3 = (Convert.ToInt32(ProductType.曳引車)).ToString();
                Page.Data5 = Page.Data.Where(p => p.ProductType == pd1value3 && p.EngineId == ed1value1).OrderBy(p => p.Orderby).ToList();
                Page.Data6 = Page.Data.Where(p => p.ProductType == pd1value3 && p.EngineId == ed1value2).OrderBy(p => p.Orderby).ToList();

                string pd1value4 = (Convert.ToInt32(ProductType.其他)).ToString();
                Page.Data7 = Page.Data.Where(p => p.ProductType == pd1value4).OrderBy(p => p.Orderby).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        public ProductInfoListModel GetList(string CurrentUserName, ProductInfoListModel Page)
        {
           var o_query = from p in basedb.product_info
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                //產品名稱(系列)
                o_query = o_query.Where(p => p.name.Contains(Page.Search));
            }

            if (!string.IsNullOrEmpty(Page.EngineId) && Page.EngineId != "0")
            {
                //產品名稱(系列)
                o_query = o_query.Where(p => p.engine_id == Page.EngineId);
            }


            if (!string.IsNullOrEmpty(Page.ProductType))
            {
                //產品名稱(系列)
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
                    o_query = o_query.OrderBy(p => p.name);
                    Page.OrderBy = 2;
                    break;
                case 2:
                    o_query = o_query.OrderByDescending(p => p.name);
                    Page.OrderBy = 1;
                    break;
                default:
                    o_query = o_query.OrderBy(p => p.orderfield).ThenByDescending(p=>p.create_time);
                    break;
            }

            var query = o_query;

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new ProductInfoModel()
                    {
						Id = o_entity.Id,
						Name = o_entity.name,
						SubTitle = o_entity.sub_title,
                        ProductType = o_entity.product_type_id,
                        EngineId = o_entity.engine_id,
						Img = o_entity.img,
						Banner = o_entity.banner,
						OverviewType = o_entity.overview_type,
						OverviewTonnesStart = o_entity.overview_tonnes_start,
						OverviewTonnesEnd = o_entity.overview_tonnes_end,
						OverviewImg = o_entity.overview_img,
						OverviewImgIntro = o_entity.overview_img_intro,
						Introduction = o_entity.introduction,
						DetailGas = o_entity.detail_gas,
						DetailDiesel = o_entity.detail_diesel,
						DetailElectric = o_entity.detail_electric,
						DetailOverlookImg = o_entity.detail_overlook_img,
						//DetailSlideImg = o_entity.detail_slide_img,
						DetailEngineTable = o_entity.detail_engine_table,
						DetailMotorBatteryTable = o_entity.detail_motor_battery_table,
						MetaKeywords = o_entity.meta_keywords,
						MetaDescription = o_entity.meta_description,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,
                        OnShelfDate = o_entity.on_shelf_date,
                        Orderby = o_entity.orderfield,
                        Status = o_entity.status,
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        

        public bool Create(string UserName, ProductInfoModel model, out string ErrMsgs)
        {
            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Img", model.ImgFile);
            }

            if (model.BannerFile != null)
            {
                Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Banner", model.BannerFile);
            }

            if (model.LogoFile != null)
            {
                Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Logo", model.LogoFile);
            }

            if (model.OverviewImgFile != null)
            {
                Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "OverviewImg", model.OverviewImgFile);
            }

            if (model.LookImgFile != null)
            {
                Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "DetailOverlookImg", model.LookImgFile);
            }

          
            //if (model.SlideImgFile != null)
            //{
            //    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "DetailSlideImg", model.SlideImgFile);
            //}

            ErrMsgs = string.Empty;

            product_info dbEntity = new product_info();
			dbEntity.Id = model.Id;
			dbEntity.name = model.Name;
			dbEntity.sub_title = model.SubTitle;
            dbEntity.product_type_id = model.ProductType;
            dbEntity.engine_id = model.EngineId;
			dbEntity.img = model.Img;
            dbEntity.logo = model.Logo;
			dbEntity.banner = model.Banner;
			dbEntity.overview_type = model.OverviewType;
			dbEntity.overview_tonnes_start = model.OverviewTonnesStart;
			dbEntity.overview_tonnes_end = model.OverviewTonnesEnd;
			dbEntity.overview_img = model.OverviewImg;
			dbEntity.overview_img_intro = model.OverviewImgIntro;
			dbEntity.introduction = model.Introduction;
			dbEntity.detail_gas = model.DetailGas;
			dbEntity.detail_diesel = model.DetailDiesel;
			dbEntity.detail_electric = model.DetailElectric;
			dbEntity.detail_overlook_img = model.DetailOverlookImg;
			//dbEntity.detail_slide_img = model.DetailSlideImg;
			dbEntity.detail_engine_table = model.DetailEngineTable;
			dbEntity.detail_motor_battery_table = model.DetailMotorBatteryTable;
			dbEntity.meta_keywords = model.MetaKeywords;
			dbEntity.meta_description = model.MetaDescription;
			dbEntity.create_time = DateTime.Now;
            dbEntity.create_id = UserName;
            dbEntity.on_shelf_date = model.OnShelfDate;
            dbEntity.overview_title = model.OverviewTitle;
            dbEntity.overview_intro = model.OverviewIntro;
            dbEntity.status = model.Status;

            basedb.product_info.Add(dbEntity);

            try
            {

                basedb.SaveChanges();

                #region img
                int index = 1;
                if (model.ImgFile1 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg1", model.ImgFile1);
                    //listimg.Add(model.ProductImg1);
                    product_info_spc_image img = new product_info_spc_image();
                    img.img = model.ProductImg1;
                    img.intro = model.ProductDesc1 == null ? string.Empty : model.ProductDesc1;
                    img.orderfield = index;
                    img.product_id = dbEntity.Id;
                    basedb.product_info_spc_image.Add(img);
                    basedb.SaveChanges();
                    index++;
                }
                if (model.ImgFile2 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg2", model.ImgFile2);
                    //listimg.Add(model.ProductImg2);
                    product_info_spc_image img = new product_info_spc_image();
                    img.img = model.ProductImg2;
                    img.intro = model.ProductDesc2 == null ? string.Empty : model.ProductDesc2;
                    img.orderfield = index;
                    img.product_id = dbEntity.Id;
                    basedb.product_info_spc_image.Add(img);
                    basedb.SaveChanges();
                    index++;
                }
                if (model.ImgFile3 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg3", model.ImgFile3);
                    //listimg.Add(model.ProductImg3);
                    product_info_spc_image img = new product_info_spc_image();
                    img.img = model.ProductImg3;
                    img.intro = model.ProductDesc3 == null ? string.Empty : model.ProductDesc3;
                    img.orderfield = index;
                    img.product_id = dbEntity.Id;
                    basedb.product_info_spc_image.Add(img);
                    basedb.SaveChanges();
                    index++;
                }
                if (model.ImgFile4 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg4", model.ImgFile4);
                    //listimg.Add(model.ProductImg4);
                    product_info_spc_image img = new product_info_spc_image();
                    img.img = model.ProductImg4;
                    img.intro = model.ProductDesc4 == null ? string.Empty : model.ProductDesc4;
                    img.orderfield = index;
                    img.product_id = dbEntity.Id;
                    basedb.product_info_spc_image.Add(img);
                    basedb.SaveChanges();
                    index++;
                }
                if (model.ImgFile5 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg5", model.ImgFile5);
                    //listimg.Add(model.ProductImg5);
                    product_info_spc_image img = new product_info_spc_image();
                    img.img = model.ProductImg5;
                    img.intro = model.ProductDesc5 == null ? string.Empty : model.ProductDesc5;
                    img.orderfield = index;
                    img.product_id = dbEntity.Id;
                    basedb.product_info_spc_image.Add(img);
                    basedb.SaveChanges();
                    index++;
                }
                if (model.ImgFile6 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg6", model.ImgFile6);
                    //listimg.Add(model.ProductImg6);
                    product_info_spc_image img = new product_info_spc_image();
                    img.img = model.ProductImg6;
                    img.intro = model.ProductDesc6 == null ? string.Empty : model.ProductDesc6;
                    img.orderfield = index;
                    img.product_id = dbEntity.Id;
                    basedb.product_info_spc_image.Add(img);
                    basedb.SaveChanges();
                    index++;
                }
                #endregion

                #region
                foreach(var i in model.AccessoriesList)
                {
                    product_info_accessories_list entity = new product_info_accessories_list();
                    entity.accessories_id = Convert.ToInt32(i);
                    entity.orderby = 0;
                    entity.product_id = dbEntity.Id;
                    basedb.product_info_accessories_list.Add(entity);
                    basedb.SaveChanges();
                }
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

        /// <summary>
        /// 取得配件列表
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetAccessories(List<string> haveList)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<int> intpList = new List<int>();
            foreach (var i in haveList)
            {
                intpList.Add(Convert.ToInt16(i));
            }

            var plist = from p in basedb.product_accessories
                        select p;

            foreach (var p in plist)
            {
                list.Add(new SelectListItem() { Value = p.Id.ToString(), Text = p.name_red + p.name_black, Selected = (intpList.Where(n => n == p.Id).Count() > 0) });
            }

            return list;
        }

        public List<SelectListItem> GetProductTypeList(string pid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString(),
                Selected = (pid == ((int)v).ToString())
            }).ToList();
            return list;
        }

        public List<SelectListItem> GetEngineList(string eid, bool isdefalut = false)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<SelectListItem> list2 = new List<SelectListItem>();
            if (isdefalut)
            {
                list2.Add(new SelectListItem() { Text = "不限", Value = "0" });
            }

            list = Enum.GetValues(typeof(ProductEngineType)).Cast<ProductEngineType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString(),
                Selected = (eid == ((int)v).ToString())
            }).ToList();
            list2.AddRange(list);
            return list2;
        }

		public ProductInfoModel Get(string userId, int? id, bool show=false)
        {
            var o_entity = (from p in basedb.product_info
                            where p.Id == id
                            select p).FirstOrDefault();
            if (show)
            {
                if (o_entity.status== false)
                {
                    o_entity = null;
                }
            }

            if (o_entity != null)
            {
                var model = new ProductInfoModel()
                    {
						Id = o_entity.Id,
						Name = o_entity.name,
						SubTitle = o_entity.sub_title,
                        ProductType = o_entity.product_type_id.ToString(),
                        EngineId = o_entity.engine_id.ToString(),
						Img = o_entity.img,
						Banner = o_entity.banner,
                        Logo =  o_entity.logo,
						OverviewType = o_entity.overview_type,
						OverviewTonnesStart = o_entity.overview_tonnes_start,
						OverviewTonnesEnd = o_entity.overview_tonnes_end,
						OverviewImg = o_entity.overview_img,
						OverviewImgIntro = o_entity.overview_img_intro,
						Introduction = o_entity.introduction,
						DetailGas = o_entity.detail_gas,
						DetailDiesel = o_entity.detail_diesel,
						DetailElectric = o_entity.detail_electric,
						DetailOverlookImg = o_entity.detail_overlook_img,
						//DetailSlideImg = o_entity.detail_slide_img,
						DetailEngineTable = o_entity.detail_engine_table,
						DetailMotorBatteryTable = o_entity.detail_motor_battery_table,
						MetaKeywords = o_entity.meta_keywords,
						MetaDescription = o_entity.meta_description,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,
                        OnShelfDate = o_entity.on_shelf_date,
                        OverviewTitle = o_entity.overview_title,
                        OverviewIntro = o_entity.overview_intro,
                        Status = o_entity.status,
                    };

                model.OverviewTonnesStartSHOW = (model.OverviewTonnesStart != 0) ? (model.OverviewTonnesStart % 1 == 0 ? model.OverviewTonnesStart.ToString(".0#") : model.OverviewTonnesStart.ToString()) : string.Empty;
                model.OverviewTonnesEndSHOW = (model.OverviewTonnesEnd != 0) ? (model.OverviewTonnesEnd % 1 == 0 ? model.OverviewTonnesEnd.ToString(".0#") : model.OverviewTonnesEnd.ToString()) : string.Empty;

                List<product_info_spc_image> arr = new List<product_info_spc_image>();

                arr = (from p in basedb.product_info_spc_image
                       where p.product_id == model.Id
                       orderby p.orderfield
                       select p).ToList();

                model.ProductImg1 = arr.Any() ? arr[0].img : string.Empty;
                model.ProductDesc1 = arr.Any() ? arr[0].intro : string.Empty;

                model.ProductImg2 = arr.Count() >= 2 ? arr[1].img : string.Empty;
                model.ProductDesc2 = arr.Count() >= 2 ? arr[1].intro : string.Empty;

                model.ProductImg3 = arr.Count() >= 3 ? arr[2].img : string.Empty;
                model.ProductDesc3 = arr.Count() >= 3 ? arr[2].intro : string.Empty;

                model.ProductImg4 = arr.Count() >= 4 ? arr[3].img : string.Empty;
                model.ProductDesc4 = arr.Count() >= 4 ? arr[3].intro : string.Empty;

                model.ProductImg5 = arr.Count() >= 5 ? arr[4].img : string.Empty;
                model.ProductDesc5 = arr.Count() >= 5 ? arr[4].intro : string.Empty;

                model.ProductImg6 = arr.Count() >= 6 ? arr[5].img : string.Empty;
                model.ProductDesc6 = arr.Count() >= 6 ? arr[5].intro : string.Empty;

                var intList =(from p in basedb.product_info_accessories_list
                                      where p.product_id == model.Id
                                      select p.accessories_id).ToList();
                model.AccessoriesList= new List<string>();
                foreach (var i in intList)
                {
                    model.AccessoriesList.Add(i.ToString());
                }

                if (string.IsNullOrEmpty(userId))
                {
                    model.ProductImgList = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(model.ProductImg1))
                        model.ProductImgList.Add(model.ProductImg1, model.ProductDesc1);
                    if (!string.IsNullOrEmpty(model.ProductImg2))
                        model.ProductImgList.Add(model.ProductImg2, model.ProductDesc2);
                    if (!string.IsNullOrEmpty(model.ProductImg3))
                        model.ProductImgList.Add(model.ProductImg3, model.ProductDesc3);
                    if (!string.IsNullOrEmpty(model.ProductImg4))
                        model.ProductImgList.Add(model.ProductImg4, model.ProductDesc4);
                    if (!string.IsNullOrEmpty(model.ProductImg5))
                        model.ProductImgList.Add(model.ProductImg5, model.ProductDesc5);
                    if (!string.IsNullOrEmpty(model.ProductImg6))
                        model.ProductImgList.Add(model.ProductImg6, model.ProductDesc6);

                    ProductAccessoriesService aService = new ProductAccessoriesService();
                    model.AccessoriesDataList = new ProductAccessoriesListModel();
                    model.AccessoriesDataList = aService.GetList1(o_entity.Id, model.AccessoriesDataList);
                }
                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, ProductInfoModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Img", model.ImgFile);
            }

            if (model.BannerFile != null)
            {
                Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Banner", model.BannerFile);
            }

            if (model.LogoFile != null)
            {
                Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Logo", model.LogoFile);
            }

            if (model.OverviewImgFile != null)
            {
                Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "OverviewImg", model.OverviewImgFile);
            }

            if (model.LookImgFile != null)
            {
                Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "DetailOverlookImg", model.LookImgFile);
            }

            try
            {
                product_info o_entity = new product_info()
                {
					Id = model.Id,
					name = model.Name,
					sub_title = model.SubTitle,
                    product_type_id = model.ProductType,
					engine_id = model.EngineId,
					img = model.Img,
                    logo = model.Logo,
					banner = model.Banner,
					overview_type = model.OverviewType,
					overview_tonnes_start = model.OverviewTonnesStart,
					overview_tonnes_end = model.OverviewTonnesEnd,
					overview_img = model.OverviewImg,
					overview_img_intro = model.OverviewImgIntro,
					introduction = model.Introduction,
					detail_gas = model.DetailGas,
					detail_diesel = model.DetailDiesel,
					detail_electric = model.DetailElectric,
					detail_overlook_img = model.DetailOverlookImg,
					//detail_slide_img = model.DetailSlideImg,
					detail_engine_table = model.DetailEngineTable,
					detail_motor_battery_table = model.DetailMotorBatteryTable,
					meta_keywords = model.MetaKeywords,
					meta_description = model.MetaDescription,
					create_time = model.CreateTime,
					create_id = model.CreateId,
                    on_shelf_date = model.OnShelfDate,
                    overview_title = model.OverviewTitle,
                    overview_intro = model.OverviewIntro,
                    status = model.Status

                };
				
                basedb.product_info.Attach(o_entity);
				basedb.Entry(o_entity).Property(x => x.create_time).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.create_id).IsModified = true;

                basedb.Entry(o_entity).State = EntityState.Modified;
                basedb.SaveChanges();

                #region 配件全刪
                var all = from p in basedb.product_info_accessories_list
                          where p.product_id == model.Id
                          select p;

                foreach (var i in all)
                {
                    basedb.product_info_accessories_list.Remove(i);
                }
                basedb.SaveChanges();
                #endregion

                #region 配件全+
                foreach (var i in model.AccessoriesList)
                {
                    product_info_accessories_list entity = new product_info_accessories_list();
                    entity.accessories_id = Convert.ToInt32(i);
                    entity.orderby = 0;
                    entity.product_id = model.Id;
                    basedb.product_info_accessories_list.Add(entity);
                    basedb.SaveChanges();
                }
                #endregion

                #region 全刪IMG
                var o_delete = from p in basedb.product_info_spc_image
                               where p.product_id == model.Id
                               orderby p.orderfield
                               select p;

                int index = 1;
                foreach (var row in o_delete)
                {
                    if (model.ProductImg1 != row.img &&
                        model.ProductImg2 != row.img &&
                        model.ProductImg3 != row.img &&
                        model.ProductImg4 != row.img &&
                        model.ProductImg5 != row.img &&
                        model.ProductImg6 != row.img)
                    {
                        basedb.product_info_spc_image.Remove(row);
                    }
                    else
                    {
                        row.orderfield = index;
                        index++;
                    }
                }
                basedb.SaveChanges();
                #endregion

                #region 全新增IMG
                if (model.ImgFile1 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg1", model.ImgFile1);
                    //listimg.Add(model.ProductImg1);
                    product_info_spc_image img1 = new product_info_spc_image();
                    img1.img = model.ProductImg1;
                    img1.intro = (string.IsNullOrEmpty(model.ProductDesc1) ? string.Empty : model.ProductDesc1);
                    img1.orderfield = index;
                    img1.product_id = model.Id;
                    basedb.product_info_spc_image.Add(img1);
                    index++;
                }
                if (model.ImgFile2 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg2", model.ImgFile2);
                    //listimg.Add(model.ProductImg2);
                    product_info_spc_image img2 = new product_info_spc_image();
                    img2.img = model.ProductImg2;
                    img2.intro = (string.IsNullOrEmpty(model.ProductDesc2) ? string.Empty : model.ProductDesc2);
                    img2.orderfield = index;
                    img2.product_id = model.Id;
                    basedb.product_info_spc_image.Add(img2);
                    index++;
                }
                if (model.ImgFile3 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg3", model.ImgFile3);
                    //listimg.Add(model.ProductImg3);
                    product_info_spc_image img3 = new product_info_spc_image();
                    img3.img = model.ProductImg3;
                    img3.intro = (string.IsNullOrEmpty(model.ProductDesc3) ? string.Empty : model.ProductDesc3);
                    img3.orderfield = index;
                    img3.product_id = model.Id;
                    basedb.product_info_spc_image.Add(img3);
                    index++;
                }
                if (model.ImgFile4 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg4", model.ImgFile4);
                    //listimg.Add(model.ProductImg4);
                    product_info_spc_image img4 = new product_info_spc_image();
                    img4.img = model.ProductImg4;
                    img4.intro = (string.IsNullOrEmpty(model.ProductDesc4) ? string.Empty : model.ProductDesc4);
                    img4.orderfield = index;
                    img4.product_id = model.Id;
                    basedb.product_info_spc_image.Add(img4);
                    index++;
                }
                if (model.ImgFile5 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg5", model.ImgFile5);
                    //listimg.Add(model.ProductImg5);
                    product_info_spc_image img5 = new product_info_spc_image();
                    img5.img = model.ProductImg5;
                    img5.intro = (string.IsNullOrEmpty(model.ProductDesc5) ? string.Empty : model.ProductDesc5);
                    img5.orderfield = index;
                    img5.product_id = model.Id;
                    basedb.product_info_spc_image.Add(img5);
                    index++;
                }
                if (model.ImgFile6 != null)
                {
                    Library.Utils.SaveFile<ProductInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ProductImg6", model.ImgFile6);
                    //listimg.Add(model.ProductImg6);
                    product_info_spc_image img6 = new product_info_spc_image();
                    img6.img = model.ProductImg6;
                    img6.intro = (string.IsNullOrEmpty(model.ProductDesc6) ? string.Empty : model.ProductDesc6);
                    img6.orderfield = index;
                    img6.product_id = model.Id;
                    basedb.product_info_spc_image.Add(img6);
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

        public bool Delete(string userName, int? id, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            var o_delete = from p in basedb.product_info
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.product_info.Remove(row);
            }

            try
            {
                basedb.SaveChanges();

                var allImg = from p in basedb.product_info_spc_image
                          where p.product_id == id
                          select p;
                foreach (var row in allImg)
                {
                    basedb.product_info_spc_image.Remove(row);
                }

                var allA = from p in basedb.product_info_accessories_list
                           where p.product_id == id
                          select p;

                foreach (var i in allA)
                {
                    basedb.product_info_accessories_list.Remove(i);
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