using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Extensions;

namespace Core.ApplicationTests.Builders
{
    public class MapperBuilder
    {
        private MapperConfiguration configuration;

        public static MapperBuilder DefaultMapper() => new();

        private MapperBuilder()
        {
            configuration = new MapperConfiguration(x => x.AddMaps(new List<Assembly>() { typeof(ApplicationServiceExtensions).Assembly }));
        }

        public MapperBuilder WithConfiguration(MapperConfiguration configuration)
        {
            this.configuration = configuration;
            return this;
        }

        public IMapper Build()
        {
            return new Mapper(configuration);
        }
    }
}
