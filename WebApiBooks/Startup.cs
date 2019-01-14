using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using BooksInfrastructure.Services;
using BooksAppCore.Services;
using BooksAppCore;
using BooksInfrastructure.EF;
using Microsoft.EntityFrameworkCore;
using BooksAppCore.Repositories;
using BooksInfrastructure.Repositories;
using System.Data.Common;
using System.Data.SqlClient;

namespace WebApiBooks
{
    public class Startup
    {
        private AuthOptions authOptions;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            authOptions = Configuration.GetSection("AuthOptions").Get<AuthOptions>();
        }

        public IConfiguration Configuration { get; }

        public DbConnection DbConnection => new SqlConnection(Configuration.GetConnectionString("MyConnection"));


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("MyConnection"));
            });

            //services.AddDbContext<MyDbContext>(opts => { opts.UseSqlServer(DbConnection); });

            services.AddTransient<IJwtService, JwtService>();  
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();

            services.Configure<AuthOptions>(Configuration);
            services.Configure<AuthOptions>(options => Configuration.GetSection("AuthOptions").Bind(options));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = System.TimeSpan.Zero
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "Book API", Version = "v1.0" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { } }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Book API v1");
            });

            app.UseMvc();
        }
    }
}
