using Owin;

namespace HangfireMultipleInstance
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HangfireConfiguration.ConfigureHangfire(app);
        }
    }
}