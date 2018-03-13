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
            var sqlConnectionString = @"Server=(myrdstest.cywxsm27o5k2.us-west-1.rds.amazonaws.com,1433);Database=Game;Integrated Security = True;";
            GlobalHost.DependencyResolver.UseSqlServer(sqlConnectionString);
            //this.ConfigureAuth(app);
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}