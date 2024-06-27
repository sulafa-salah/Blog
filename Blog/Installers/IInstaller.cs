namespace Blog.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env);
    }
}
