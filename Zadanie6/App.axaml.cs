﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Zadanie6.Data;
using Zadanie6.Models;
using Zadanie6.ViewModels;
using Zadanie6.Views;

namespace Zadanie6;

public partial class App : Application
{
    public new static App Current => (App)Application.Current;
    public IServiceProvider Services { get; private set; }
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddDbContext<ZakupyZadanie6Context>(options => options.UseSqlite("Data Source=ZakupyZadanie6.db"));
        services.AddTransient<MainViewModel>();
        return services.BuildServiceProvider();
    }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Services = ConfigureServices();
        if (!Design.IsDesignMode)
        {
            using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ZakupyZadanie6Context>();
                context.Database.Migrate();
                if (!context.MyProperty.Any())
                {
                    context.Add(new Purchase()
                    {
                        Title = "Mleko",
                        DateTime = DateTime.Now,
                        UnitPrice = 2.8m,
                        Amount = 2.5m
                    });
                    context.SaveChanges();
                }
            }
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainViewModel>()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = Services.GetRequiredService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
