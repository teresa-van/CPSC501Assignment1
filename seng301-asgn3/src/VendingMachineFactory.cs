using System;
using System.Collections.Generic;
using Frontend2;
using Frontend2.Hardware;

public class VendingMachineFactory : IVendingMachineFactory {

    List<VendingMachine> vMs;

    public VendingMachineFactory() {
        this.vMs = new List<VendingMachine>();
    }

    public int CreateVendingMachine(List<int> cK, int sBC, int cRC, int pRC, int recC) {
        var coinKindArray = cK.ToArray();
        var vm = new VendingMachine(coinKindArray, sBC, cRC, pRC, recC);
        this.vMs.Add(vm);
        new VendingMachineLogic(vm);
        return this.vMs.Count - 1;
    }

    public void ConfigureVendingMachine(int vmIndex, List<string> popNames, List<int> popCosts) {
        var vm = this.vMs[vmIndex];
        vm.Configure(popNames, popCosts);
    }

    public void LoadCoins(int vmIndex, int cKI, List<Coin> coins) {
        this.vMs[vmIndex].CoinRacks[cKI].LoadCoins(coins);
    }

    public void LoadPops(int vmIndex, int popKindIndex, List<PopCan> pops) {
        this.vMs[vmIndex].PopCanRacks[popKindIndex].LoadPops(pops);
    }

    public void InsertCoin(int vmIndex, Coin coin) {
        this.vMs[vmIndex].CoinSlot.AddCoin(coin);
    }

    public void PressButton(int vmIndex, int value) {
        this.vMs[vmIndex].SelectionButtons[value].Press();
    }

    public List<IDeliverable> ExtractStuff(int vmIndex) {
        var vm = this.vMs[vmIndex];
        var items = vm.DeliveryChute.RemoveItems();
        var itemsAsList = new List<IDeliverable>(items);

        return itemsAsList;
    }

    public vMstoredContents UnloadVendingMachine(int vmIndex) {
        var storedContents = new vMstoredContents();
        var vm = this.vMs[vmIndex];

        foreach(var coinRack in vm.CoinRacks) {
            storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
        }
        storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());
        foreach(var popCanRack in vm.PopCanRacks) {
            storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
        }
                
        return storedContents;
    }
}