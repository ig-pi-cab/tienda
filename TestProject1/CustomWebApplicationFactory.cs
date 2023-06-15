using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IMapper> IMapperMock { get; }
        public Mock<IUnitOfWork> IUnitOfWorkMock { get; }

        public CustomWebApplicationFactory()
        {
            IMapperMock = new Mock<IMapper>();
            IUnitOfWorkMock = new Mock<IUnitOfWork>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(IMapperMock.Object);
                services.AddSingleton(IUnitOfWorkMock.Object);
            });
        }
    }
}
