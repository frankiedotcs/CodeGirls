using Owin;
using Microsoft.Owin;
using System;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(SignalRChat.Startup))]
namespace SignalRChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string sqlConnectionString = @"Server=myrdstest.cywxsm27o5k2.us-west-1.rds.amazonaws.com,1433;Database=Game;User Id=adminGame; password=CS3750account";
            GlobalHost.DependencyResolver.UseSqlServer(sqlConnectionString);
            //this.ConfigureAuth(app);
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}