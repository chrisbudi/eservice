using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using E.Service.Resource.Api.Client;
using E.Service.Resource.Api.Component;
using E.Service.Resource.Api.Services;
using E.Service.Resource.Api.Services.Approval;
using E.Service.Resource.Api.Services.Asset;
using E.Service.Resource.Api.Services.Car;
using E.Service.Resource.Api.Services.Core;
using E.Service.Resource.Api.Services.Dashboard;
using E.Service.Resource.Api.Services.Meeting;
using E.Service.Resource.Api.Services.Order;
using E.Service.Resource.Api.Services.Order.Request;
using E.Service.Resource.Api.Services.Repair;
using E.Service.Resource.Api.Services.Report;
using E.Service.Resource.Api.Services.Travel;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Approval;
using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Core;
using E.Service.Resource.Data.Interface.Dashboard;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Order;
using E.Service.Resource.Data.Interface.Repair;
using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Travel;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Sample.Core.Resource.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string GetXmlCommentsPath()
        {
            var app = AppContext.BaseDirectory;
            return System.IO.Path.Combine(app, "E.Service.Resource.Api.xml");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {



            #region Add CORS
            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader().AllowCredentials();
            }));
            #endregion

            #region Add Authentication
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signingKey,
                    ValidateAudience = true,
                    ValidAudience = this.Configuration["Tokens:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = this.Configuration["Tokens:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
            #endregion

            //json options
            services.AddMvc().AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            /// Add swager
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.DescribeAllEnumsAsStrings();

                c.SwaggerDoc("meeting_v1", new Info
                {
                    Title = "Module Ruang Meeting",
                    Version = "v1",
                    Description = "Meeting api meeting version 1",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Chris", Email = "chris_budi@outlook.com" },
                });

                c.SwaggerDoc("car_v1", new Info
                {
                    Title = "Module Car",
                    Version = "v1",
                    Description = "Meeting api meeting version 1",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Chris", Email = "chris_budi@outlook.com" },
                });

                c.SwaggerDoc("main_v1", new Info
                {
                    Title = "Main modul gagas",
                    Version = "v1",
                    Description = "Api Main Modul version 1",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Chris", Email = "chris_budi@outlook.com" },
                });

                c.SwaggerDoc("asset_v1", new Info
                {
                    Title = "Asset",
                    Version = "v1",
                    Description = "Asset module",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Chris", Email = "chris_budi@outlook.com" },
                });

                c.SwaggerDoc("order_v1", new Info
                {
                    Title = "Ordering module",
                    Version = "v1",
                    Description = "Ordering module version 1",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Chris", Email = "chris_budi@outlook.com" },
                });

                c.SwaggerDoc("travel_v1", new Info
                {
                    Title = "Travel module",
                    Version = "v1",
                    Description = "travel module version 1",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Chris", Email = "chris_budi@outlook.com" }
                });

                c.SwaggerDoc("repair_v1", new Info
                {
                    Title = "Repair module",
                    Version = "v1",
                    Description = "repair module version 1",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Chris", Email = "chris_budi@outlook.com" },
                });

                c.SwaggerDoc("report_v1", new Info
                {
                    Title = "Report module",
                    Version = "v1",
                    Description = "report module version 1",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Chris", Email = "chris_budi@outlook.com" },
                });

                c.SwaggerDoc("dashboard_v1", new Info
                {
                    Title = "dashboard module",
                    Version = "v1",
                    Description = "dashboard module version 1",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Chris", Email = "chris_budi@outlook.com" },
                });

            });


            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));


            //connection string
            var connectionString = Configuration.GetSection("ConnectionString:SqlServerConnection");

            services.AddDbContext<EservicesdbContext>(options =>
                options.UseSqlServer(connectionString.Value.ToString()));



            services.AddScoped<EservicesdbContext, EservicesdbContext>();
            services.AddScoped<IUserGroupRoleService, UserGroupRoleService>();
            services.AddScoped<IUserTargetService, UserTargetService>();

            //core service
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IBudgetRoleService, BudgetRoleService>();
            services.AddScoped<IJenisRoleService, JenisRoleService>();
            services.AddScoped<IGroupService, GroupService>();

            //car pools data
            services.AddScoped<ICarPoolService, CarPoolService>();
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<ICarRequestService, CarRequestService>();
            services.AddScoped<ICarCoordinateStatusService, CarCoordinateStatusService>();
            services.AddScoped<ICarCoordinateService, CarCoordinateService>();



            //Meeting service
            //services.AddScoped<IOrganizationService, OrganizationServices>();
            services.AddScoped<ILocationTypeService, LocationTypeService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IMeetingRoomCategoryService, MeetingRoomCategoryService>();
            services.AddScoped<IMeetingTypeService, MeetingTypeService>();
            services.AddScoped<IMeetingTypeService, MeetingTypeService>();
            services.AddScoped<IRoomsService, RoomsService>();
            services.AddScoped<IMeetingRoomService, MeetingRoomService>();
            services.AddScoped<IJabatanService, JabatanService>();
            services.AddScoped<IMeetingBudgetService, MeetingBudgetService>();
            services.AddScoped<IRegionService, RegionService>();


            //order Service
            services.AddScoped<IOrderStationaryService, OrderStationaryService>();
            services.AddScoped<IOrderInventoryService, OrderInventoryService>();
            services.AddScoped<IOrderPrintingService, OrderPrintingService>();
            services.AddScoped<IOrderRequestService, OrderRequestService>();
            services.AddScoped<IOrderReloadService, OrderReloadService>();


            //System Service
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IApprovalService, ApprovalService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IMeetingRequest, MeetingRequestService>();
            services.AddScoped<IMeetingRequestDashboardService, MeetingRequestDashboardService>();
            services.AddScoped<IHBBDashboardService, HBBDashboardService>();
            services.AddScoped<ICarDashboardService, CarDashboardService>();




            //Asset 
            services.AddScoped<IAssetBrandService, AssetBrandService>();
            services.AddScoped<IAssetGroupService, AssetGroupService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IAssetTypeService, AssetTypeService>();
            services.AddScoped<IAssetBorrowService, AssetBorrowService>();


            //repair
            services.AddScoped<IRepairItemService, RepairItemService>();
            services.AddScoped<IRepairItemRequestService, RepairItemRequestService>();

            //Travel
            services.AddScoped<ITravelCitiesService, TravelCitiesService>();
            services.AddScoped<ITravelHotelService, TravelHotelService>();
            services.AddScoped<ITravelOutbondTypeService, TravelOutbondService>();
            services.AddScoped<ITravelRequestService, TravelRequestService>();
            services.AddScoped<ITravelTransportationService, TravelTransportService>();
            services.AddScoped<ITravelTransportationTypeService, TravelTransportationTypeService>();
            services.AddScoped<ITravelRequestAccountabilityService, TravelRequestAccountabilityService>();


            //report
            services.AddScoped<IMeetingRequestReportService, ReportMeetingRequestService>();
            services.AddScoped<ITravelRequestReport, ReportTravelService>();
            services.AddScoped<IAssetBorrowReport, ReportBorrowService>();
            services.AddScoped<IAssetReport, ReportAssetService>();
            services.AddScoped<IOrderReloadReport, ReportOrderReloadService>();
            services.AddScoped<IOrderRequestReport, ReportOrderRequestService>();
            services.AddScoped<IRepairReport, ReportRepairService>();


            //http client
            services.AddHttpClient<EprocClient>();
            services.AddHttpClient<ExpoClient>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddSingleton<EmailSettings, EmailSettings>();

            services.AddMvc();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Startup.Configure(IApplicationBuilder, IHostingEnvironment)'
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Startup.Configure(IApplicationBuilder, IHostingEnvironment)'
        {
            app.UseCors("Cors");
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            app.UseStaticFiles();
            app.UseSwagger();
            //app.UseLogging();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/main_v1/swagger.json", "Modul Main/Core");
                c.SwaggerEndpoint("/swagger/meeting_v1/swagger.json", "Modul Meeting");
                c.SwaggerEndpoint("/swagger/asset_v1/swagger.json", "Modul Asset");
                c.SwaggerEndpoint("/swagger/order_v1/swagger.json", "Modul Order");
                c.SwaggerEndpoint("/swagger/car_v1/swagger.json", "Modul Car");
                c.SwaggerEndpoint("/swagger/travel_v1/swagger.json", "Modul travel");
                c.SwaggerEndpoint("/swagger/repair_v1/swagger.json", "Modul repair");
                c.SwaggerEndpoint("/swagger/report_v1/swagger.json", "Modul report");
                c.SwaggerEndpoint("/swagger/dashboard_v1/swagger.json", "Modul Dashboard");
                c.RoutePrefix = string.Empty;
                c.DisplayOperationId();
                c.DefaultModelExpandDepth(0);
                c.DefaultModelsExpandDepth(-1);
            });

            app.UseMvc();
        }
    }
}
