using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zadanie6.Data;
using Zadanie6.Models;

namespace Zadanie6.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ZakupyZadanie6Context context;
    public MainViewModel(ZakupyZadanie6Context context)
    {
        this.context = context;
        Purchases = context.MyProperty
    .LoadAsync()
    .ContinueWith(p => context.MyProperty.Local.ToObservableCollection());
    }
    [RelayCommand]
    private async Task AddPurchaseAsync()
    {
        if (NewPurchase.Validate())
        {
            context.Add(NewPurchase);
            await context.SaveChangesAsync();
            NewPurchase = new Purchase();
        }
    }
    [RelayCommand]
    private async Task DeletePurchaseAsync(Purchase purchase)
    {
        context.Remove(purchase);
        await context.SaveChangesAsync();
    }

    [ObservableProperty]
    private Purchase newPurchase = new Purchase();
    private TaskNotifier<ObservableCollection<Purchase>> purchases;
    public Task<ObservableCollection<Purchase>> Purchases
    {
        get => purchases;
        set => SetPropertyAndNotifyOnCompletion(ref purchases, value);
    }
}
