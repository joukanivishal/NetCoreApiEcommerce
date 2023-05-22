# NetCoreApiEcommerce

This is sample project to learn ASP.NET 7 API and other stuff such as Authentication using JWT,message broker such as RabbitMq, Reddis as cache..etc

*********************Add Ocelot ApiGateway*********************************
Find and add latest Ocelot package from manage nuget package to empty api project
Add new json file to project named as "ocelot.json"
In ocelot.json add "Routes" and in Routes add Downstream & Upstream as shown below: 
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/GetWeatherForecast",
      "DownstreamScheme": "IISExpress",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 44374
        }
      ],
      "UpstreamPathTemplate": "/WeatherForecast",
      "UpstreamHttpMethod": ["Get"]
    }
  ]
}

Here DownstreamPathTemplate is method name of our internal api which we want to point
when call /WeatherForecast from our api gateway project which is mentioned in UpstreamPathTemplate.
DownstreamScheme is either http or https on which our internal application is running and DownstreamHostAndPorts are host and port where our api will run.

Once our json file is set, go to program file and before builder.Build() write below code to include ocelot.json file.

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange:true);
builder.Services.AddOcelot(builder.Configuration);

And in request pipeline start using ocelot middleware below controller as app.UseOcelot(); or await app.UseOcelot();
*********************XXXX****************************************************************
