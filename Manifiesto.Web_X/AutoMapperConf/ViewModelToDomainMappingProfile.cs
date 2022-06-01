using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Manifiesto.Web.Areas.Aereo.Models;
using Manifiesto.Data.Models;
using Manifiesto.Web.Areas.Maritimo.Models;
using master_aimar.Entities.Entities;
using db_aereo.Entities.Entities;

namespace Manifiesto.Web.AutoMapperConf
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<manifiesto_encVM, cuscar_voyage_info>();
            Mapper.CreateMap<manifiestoEncViewModel, cuscar_voyage_info>();
            Mapper.CreateMap<manifiestoDetViewModel, cuscar_bl_info>();
            //Mapper.CreateMap<manifiesto_encVM, manifiesto_enc>();
            //Mapper.CreateMap<aamnViewModel, aamn>();
            //Mapper.CreateMap<aablVM, aabl>();
            //Mapper.CreateMap<guiaImportViewModel, awbi>();

            Mapper.CreateMap<container_typeViewModel, container_type>();
        }
    }
}