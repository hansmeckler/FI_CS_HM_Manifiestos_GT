using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Manifiesto.Web.Areas.Aereo.Models;
using Manifiesto.Data.Models;
using master_aimar.Entities.Entities;
using Manifiesto.Web.Areas.Maritimo.Models;
using db_aereo.Entities.Entities;

namespace Manifiesto.Web.AutoMapperConf
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<cuscar_voyage_info, manifiesto_encVM>();
            Mapper.CreateMap<cuscar_voyage_info, manifiestoEncViewModel>();
            Mapper.CreateMap<cuscar_bl_info, manifiestoDetViewModel>();
            /*
            Mapper.CreateMap<awbi, guiaImportViewModel>();

            Mapper.CreateMap<manifiesto_enc, manifiestoEncViewModel>();
            Mapper.CreateMap<manifiesto_enc, manifiesto_encVM>();
            Mapper.CreateMap<manifiesto_det, manifiestoDetViewModel>();
            Mapper.CreateMap<awbi, guiaImportViewModel>();
             * */
            Mapper.CreateMap<container_type, container_typeViewModel>();
        }
    }
}