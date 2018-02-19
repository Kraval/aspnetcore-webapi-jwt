# aspnetcore-webapi-jwt

This repository shows a reference implementation of Asp .Net Core 2.0 Web API secured by Asp .Net Identity and JWT authentication. Following is a setp by step instruction if you want to do it all yourself.

## Create Initial Web Api Project and add Necessary Tooling

```
mkdir JwtApi

cd JwtApi

dotnet new webapi

code .
```

Enable the vscode build & debug configuration 

![Basic NG5 Cli App](_images/1_Add_Build.png "Add Build")

```
dotnet restore
```

Next, let's add following packages to our Web API project.

```
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 2.0.1

dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 2.0.1

dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNet --version 2.0.1
```

Open the .csproj file and add following Package and CLiToolReference. Unfortunately there is no CLI support yet to add CLiToolReference item using command line.

```
<DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.1" />
```

Next restore newly referred packages with `dotnet restore`


## Add Asp .Net Identity and Configure SqLite as a DataStore

Add a new ApplicationUser.cs file as below.

```
using System;
using Microsoft.AspNetCore.Identity;

namespace AspnetCoreWebApiJwt.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Url {get;set;}
    }
}
```

Add ApplicationDbContext.cs file as below.

```
using AspNetCoreWebApiJwt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApiJwt.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
              : base(options)
        {
        }
    }
}
```

Wrre up Asp .Net Identity and ApplicationDbContext under ConfigureServies method in Startup.cs file.

```
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            //Configure EF Core DbContext for SqLite Database
            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlite("Filename=./AspnetCoreWebApiJwt.db"));

            
            //Setup Asp .Net Identity With Options
            var builder = services.AddIdentityCore<ApplicationUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddAutoMapper();
        }
```

Head over to your Command prompt and Add/Enabble Initial migration and Update database. In our case, if everything works as expected it should generate AspnetCoreWebApiJwt.db file.

```
dotnet ef migrations add initial

dotnet ef database update
```












dotnet restsore

