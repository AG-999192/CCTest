using AutoMapper;
using C_C_Test.Dtos;
using C_C_Test.Models;

namespace C_C_Test.Conversions
{
    public class Conversion : IConversion
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        public Conversion()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RetrievedData, DataViewModel>()
                    .ForMember(dst => dst.MPAN, opt => opt.MapFrom(src => src.MPAN))
                    .ForMember(dst => dst.MeterSerial, opt => opt.MapFrom(src => src.MeterSerial))
                    .ForMember(dst => dst.DateOfInstallation, opt => opt.MapFrom(src => src.DateOfInstallation))
                    .ForMember(dst => dst.AddressLine, opt => opt.MapFrom(src => src.AddressLine))
                    .ForMember(dst => dst.PostCode, opt => opt.MapFrom(src => src.PostCode));
            });

            this.mapper = config.CreateMapper();
        }

        public List<DataViewModel> MapRetrievedDataToDataView(List<RetrievedData> manifests)
        {
            return this.mapper.Map<List<RetrievedData>, List<DataViewModel>>(manifests);
        }
    }
}
