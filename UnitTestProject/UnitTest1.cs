using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frontend2;
using Frontend2.Hardware;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void T01GoodInsertAndPressExactChange()
        {
            //CREATE
            int[] coins = { 5, 10, 25, 100 };
            VendingMachine vm = new VendingMachine(coins, 3, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("Coke"); popNames.Add("water"); popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(250); popCosts.Add(250); popCosts.Add(205);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[0].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("Coke"));
            vm.PopCanRacks[0].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("water"));
            vm.PopCanRacks[1].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[2].LoadPops(loadpops);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(25)); vm.CoinSlot.AddCoin(new Coin(25));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            expectedPops.Add(new PopCan("Coke"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 315;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();
            unsoldExpected.Add("water"); unsoldExpected.Add("stuff");

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }

        [TestMethod]
        public void T02GoodInsertAndPressChangeExpected()
        {
            //CREATE
            int[] coins = { 5, 10, 25, 100 };
            VendingMachine vm = new VendingMachine(coins, 3, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("Coke"); popNames.Add("water"); popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(250); popCosts.Add(250); popCosts.Add(205);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[0].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("Coke"));
            vm.PopCanRacks[0].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("water"));
            vm.PopCanRacks[1].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[2].LoadPops(loadpops);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            expectedPops.Add(new PopCan("Coke"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(50, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 315;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();
            unsoldExpected.Add("water"); unsoldExpected.Add("stuff");

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }

        [TestMethod]
        public void T03GoodTeardownWithoutConfigureOrLoad()
        {
            //CREATE
            int[] coins = { 5, 10, 25, 100 };
            VendingMachine vm = new VendingMachine(coins, 3, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 0;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }

        [TestMethod]
        public void T04GoodPressWithoutInsert()
        {
            //CREATE
            int[] coins = { 5, 10, 25, 100 };
            VendingMachine vm = new VendingMachine(coins, 3, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("Coke"); popNames.Add("water"); popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(250); popCosts.Add(250); popCosts.Add(205);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[0].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("Coke"));
            vm.PopCanRacks[0].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("water"));
            vm.PopCanRacks[1].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[2].LoadPops(loadpops);

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 65;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();
            unsoldExpected.Add("Coke"); unsoldExpected.Add("water"); unsoldExpected.Add("stuff");

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }

        [TestMethod]
        public void T05GoodScrambledCoinKinds()
        {
            //CREATE
            int[] coins = { 100, 5, 25, 10 };
            VendingMachine vm = new VendingMachine(coins, 3, 2, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("Coke"); popNames.Add("water"); popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(250); popCosts.Add(250); popCosts.Add(205);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10));
            vm.CoinRacks[3].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("Coke"));
            vm.PopCanRacks[0].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("water"));
            vm.PopCanRacks[1].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[2].LoadPops(loadpops);

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            change = 0;
            popsVended.Clear();
            expectedPops.Clear();
            expectedPops.Add(new PopCan("Coke"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(50, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 215;
            int storageExpected = 100;
            List<String> unsoldExpected = new List<String>();
            unsoldExpected.Add("water"); unsoldExpected.Add("stuff");

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }

        [TestMethod]
        public void T06GoodExtractBeforeSale()
        {
            //CREATE
            int[] coins = { 100, 5, 25, 10 };
            VendingMachine vm = new VendingMachine(coins, 3, 2, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("Coke"); popNames.Add("water"); popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(250); popCosts.Add(250); popCosts.Add(205);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10));
            vm.CoinRacks[3].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("Coke"));
            vm.PopCanRacks[0].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("water"));
            vm.PopCanRacks[1].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[2].LoadPops(loadpops);

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            //EXTRACT
            deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            change = 0;
            popsVended.Clear();
            expectedPops.Clear();
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 65;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();
            unsoldExpected.Add("Coke"); unsoldExpected.Add("water"); unsoldExpected.Add("stuff");

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }

        [TestMethod]
        public void T07GoodChangingConfiguration()
        {
            //CREATE
            int[] coins = { 5, 10, 25, 100 };
            VendingMachine vm = new VendingMachine(coins, 3, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("A"); popNames.Add("B"); popNames.Add("C");
            List<int> popCosts = new List<int>();
            popCosts.Add(5); popCosts.Add(10); popCosts.Add(25);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[0].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("A"));
            vm.PopCanRacks[0].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("B"));
            vm.PopCanRacks[1].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("C"));
            vm.PopCanRacks[2].LoadPops(loadpops);

            //CONFIGURE
            popNames = new List<String>();
            popNames.Add("Coke"); popNames.Add("water"); popNames.Add("stuff");
            popCosts = new List<int>();
            popCosts.Add(250); popCosts.Add(250); popCosts.Add(205);
            vm.Configure(popNames, popCosts);

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            change = 0;
            popsVended = new List<PopCan>();
            expectedPops = new List<PopCan>();
            expectedPops.Add(new PopCan("A"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(50, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 315;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();
            unsoldExpected.Add("B"); unsoldExpected.Add("C");

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);

            //LOAD COINS
            loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[0].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);

            //LOAD POPS
            loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("Coke"));
            vm.PopCanRacks[0].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("water"));
            vm.PopCanRacks[1].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[2].LoadPops(loadpops);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            change = 0;
            popsVended = new List<PopCan>();
            expectedPops = new List<PopCan>();
            expectedPops.Add(new PopCan("Coke"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(50, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            rackExpected = 315;
            storageExpected = 0;
            unsoldExpected = new List<String>();
            unsoldExpected.Add("water"); unsoldExpected.Add("stuff");

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }

        [TestMethod]
        public void T08GoodApproximateChange()
        {
            //CREATE
            int[] coins = { 5, 10, 25, 100 };
            VendingMachine vm = new VendingMachine(coins, 1, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(140);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(100));
            vm.CoinRacks[3].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[0].LoadPops(loadpops);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            expectedPops.Add(new PopCan("stuff"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(155, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 320;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }

        [TestMethod]
        public void T09GoodHardForChange()
        {
            //CREATE
            int[] coins = { 5, 10, 25, 100 };
            VendingMachine vm = new VendingMachine(coins, 1, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(140);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[0].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(100));
            vm.CoinRacks[3].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[0].LoadPops(loadpops);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            expectedPops.Add(new PopCan("stuff"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(160, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 330;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }

        [TestMethod]
        public void T10GoodInvalidCoin()
        {
            //CREATE
            int[] coins = { 5, 10, 25, 100 };
            VendingMachine vm = new VendingMachine(coins, 1, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(140);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[0].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(100));
            vm.CoinRacks[3].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[0].LoadPops(loadpops);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(1));
            vm.CoinSlot.AddCoin(new Coin(139));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(140, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 190;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();
            unsoldExpected.Add("stuff");

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }

        [TestMethod]
        public void T11GoodExtractBeforeSaleComplex()
        {
            //CREATE
            int[] coins = { 100, 5, 25, 10 };
            VendingMachine vm = new VendingMachine(coins, 3, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("Coke"); popNames.Add("water"); popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(250); popCosts.Add(250); popCosts.Add(205);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10));
            vm.CoinRacks[3].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("Coke"));
            vm.PopCanRacks[0].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("water"));
            vm.PopCanRacks[1].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[2].LoadPops(loadpops);

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            //EXTRACT
            deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            change = 0;
            popsVended = new List<PopCan>();
            expectedPops = new List<PopCan>();
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 65;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();
            unsoldExpected.Add("Coke"); unsoldExpected.Add("water"); unsoldExpected.Add("stuff");

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);

            //LOAD COINS
            loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10));
            vm.CoinRacks[3].LoadCoins(loadcoins);

            //LOAD POPS
            loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("Coke"));
            vm.PopCanRacks[0].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("water"));
            vm.PopCanRacks[1].LoadPops(loadpops);
            loadpops.Clear();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[2].LoadPops(loadpops);

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            change = 0;
            popsVended = new List<PopCan>();
            expectedPops = new List<PopCan>();
            expectedPops.Add(new PopCan("Coke"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(50, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            rackExpected = 315;
            storageExpected = 0;
            unsoldExpected = new List<String>();
            unsoldExpected.Add("water"); unsoldExpected.Add("stuff");

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);

            //CREATE
            int[] coins2 = { 100, 5, 25, 10 };
            VendingMachine vm2 = new VendingMachine(coins, 3, 10, 10, 10);
            VendingMachineLogic vmLogic2 = new VendingMachineLogic(vm2);

            //CONFIGURE
            List<String> popNames2 = new List<String>();
            popNames2.Add("Coke"); popNames2.Add("water"); popNames2.Add("stuff");
            List<int> popCosts2 = new List<int>();
            popCosts2.Add(250); popCosts2.Add(250); popCosts2.Add(205);
            vm2.Configure(popNames2, popCosts2);

            //CONFIGURE
            popNames2 = new List<String>();
            popNames2.Add("A"); popNames2.Add("B"); popNames2.Add("C");
            popCosts2 = new List<int>();
            popCosts2.Add(5); popCosts2.Add(10); popCosts2.Add(25);
            vm2.Configure(popNames2, popCosts2);

            //UNLOAD
            int coinrackstotal2 = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal2 += coin.Value;
                }
            }

            int storagebintotal2 = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal2 += coin.Value;
            }

            List<String> unsoldpoplist2 = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist2.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected2 = 0;
            int storageExpected2 = 0;
            List<String> unsoldExpected2 = new List<String>();

            Assert.AreEqual(rackExpected2, coinrackstotal2);
            Assert.AreEqual(storageExpected2, storagebintotal2);
            CollectionAssert.AreEqual(unsoldExpected2, unsoldpoplist2);

            //LOAD COINS
            List<Coin> loadcoins2 = new List<Coin>();
            loadcoins2.Add(new Coin(5));
            vm2.CoinRacks[1].LoadCoins(loadcoins2);
            loadcoins2.Clear();
            loadcoins2.Add(new Coin(25)); loadcoins2.Add(new Coin(25));
            vm2.CoinRacks[2].LoadCoins(loadcoins2);
            loadcoins2.Clear();
            loadcoins2.Add(new Coin(10));
            vm2.CoinRacks[3].LoadCoins(loadcoins2);

            //LOAD POPS
            List<PopCan> loadpops2 = new List<PopCan>();
            loadpops2.Add(new PopCan("A"));
            vm2.PopCanRacks[0].LoadPops(loadpops2);
            loadpops2.Clear();
            loadpops2.Add(new PopCan("B"));
            vm2.PopCanRacks[1].LoadPops(loadpops2);
            loadpops2.Clear();
            loadpops2.Add(new PopCan("C"));
            vm2.PopCanRacks[2].LoadPops(loadpops2);

            //INSERT COINS
            vm2.CoinSlot.AddCoin(new Coin(10));
            vm2.CoinSlot.AddCoin(new Coin(5));
            vm2.CoinSlot.AddCoin(new Coin(10));

            //PRESS BUTTON
            vm2.SelectionButtons[2].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute2 = new List<IDeliverable>(vm2.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change2 = 0;
            List<PopCan> popsVended2 = new List<PopCan>();
            List<PopCan> expectedPops2 = new List<PopCan>();
            expectedPops2.Add(new PopCan("C"));
            foreach (IDeliverable thing in deliveryChute2)
            {
                if (thing is Coin)
                {
                    change2 += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended2.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change2);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            coinrackstotal2 = 0;
            foreach (CoinRack coinrack in vm2.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal2 += coin.Value;
                }
            }

            storagebintotal2 = 0;
            foreach (Coin coin in vm2.StorageBin.Unload())
            {
                storagebintotal2 += coin.Value;
            }

            unsoldpoplist2 = new List<String>();
            foreach (PopCanRack poprack in vm2.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist2.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            rackExpected2 = 90;
            storageExpected2 = 0;
            unsoldExpected2 = new List<String>();
            unsoldExpected2.Add("A"); unsoldExpected2.Add("B");

            Assert.AreEqual(rackExpected2, coinrackstotal2);
            Assert.AreEqual(storageExpected2, storagebintotal2);
            CollectionAssert.AreEqual(unsoldExpected2, unsoldpoplist2);
        }

        [TestMethod]
        public void T12GoodApproximateChangeWithCredit()
        {
            //CREATE
            int[] coins = { 5, 10, 25, 100 };
            VendingMachine vm = new VendingMachine(coins, 1, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(140);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(100));
            vm.CoinRacks[3].LoadCoins(loadcoins);

            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[0].LoadPops(loadpops);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(100));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            expectedPops.Add(new PopCan("stuff"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(change, 155, "A");
            CollectionAssert.AreEqual(expectedPops, popsVended, "B");

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            int rackExpected = 320;
            int storageExpected = 0;
            List<String> unsoldExpected = new List<String>();

            Assert.AreEqual(coinrackstotal, rackExpected, "C");
            Assert.AreEqual(storagebintotal, storageExpected, "D");
            CollectionAssert.AreEqual(unsoldpoplist, unsoldExpected, "E");

            //LOAD COINS
            loadcoins.Clear();
            loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5));
            loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5));
            vm.CoinRacks[0].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100));
            loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100));
            vm.CoinRacks[3].LoadCoins(loadcoins);

            //LOAD POPS
            loadpops.Clear();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[0].LoadPops(loadpops);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(25)); vm.CoinSlot.AddCoin(new Coin(100)); vm.CoinSlot.AddCoin(new Coin(10));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            change = 0;
            popsVended.Clear();
            expectedPops.Clear();
            expectedPops.Add(new PopCan("stuff"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(change, 0, "F");
            CollectionAssert.AreEqual(expectedPops, popsVended, "G");

            //UNLOAD
            coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            unsoldpoplist.Clear();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            rackExpected = 1400;
            storageExpected = 135;
            unsoldExpected.Clear();

            Assert.AreEqual(coinrackstotal, rackExpected, "H");
            Assert.AreEqual(storagebintotal, storageExpected, "I");
            CollectionAssert.AreEqual(unsoldpoplist, unsoldExpected, "J");
        }

        [TestMethod]
        public void T13GoodNeedToStorePayment()
        {
            //CREATE
            int[] coins = { 5, 10, 25, 100 };
            VendingMachine vm = new VendingMachine(coins, 1, 10, 10, 10);
            VendingMachineLogic vmLogic = new VendingMachineLogic(vm);

            //CONFIGURE
            List<String> popNames = new List<String>();
            popNames.Add("stuff");
            List<int> popCosts = new List<int>();
            popCosts.Add(135);
            vm.Configure(popNames, popCosts);

            //LOAD COINS
            List<Coin> loadcoins = new List<Coin>();
            loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5));
            loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5));
            loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5));
            loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5));
            loadcoins.Add(new Coin(5)); loadcoins.Add(new Coin(5));
            vm.CoinRacks[0].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            loadcoins.Add(new Coin(10)); loadcoins.Add(new Coin(10));
            vm.CoinRacks[1].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            loadcoins.Add(new Coin(25)); loadcoins.Add(new Coin(25));
            vm.CoinRacks[2].LoadCoins(loadcoins);
            loadcoins.Clear();
            loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100));
            loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100));
            loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100));
            loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100));
            loadcoins.Add(new Coin(100)); loadcoins.Add(new Coin(100));
            vm.CoinRacks[3].LoadCoins(loadcoins);


            //LOAD POPS
            List<PopCan> loadpops = new List<PopCan>();
            loadpops.Add(new PopCan("stuff"));
            vm.PopCanRacks[0].LoadPops(loadpops);

            //INSERT COINS
            vm.CoinSlot.AddCoin(new Coin(25)); vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(10));

            //PRESS BUTTON
            vm.SelectionButtons[0].Press();

            //EXTRACT
            List<IDeliverable> deliveryChute = new List<IDeliverable>(vm.DeliveryChute.RemoveItems());

            //CHECK DELIVERY
            int change = 0;
            List<PopCan> popsVended = new List<PopCan>();
            List<PopCan> expectedPops = new List<PopCan>();
            expectedPops.Add(new PopCan("stuff"));
            foreach (IDeliverable thing in deliveryChute)
            {
                if (thing is Coin)
                {
                    change += (Convert.ToInt16(thing.ToString()));
                }
                if (thing is PopCan)
                {
                    popsVended.Add(new PopCan(thing.ToString()));
                }
            }
            Assert.AreEqual(0, change);
            CollectionAssert.AreEqual(expectedPops, popsVended);

            //UNLOAD
            int coinrackstotal = 0;
            foreach (CoinRack coinrack in vm.CoinRacks)
            {
                foreach (Coin coin in coinrack.Unload())
                {
                    coinrackstotal += coin.Value;
                }
            }

            int storagebintotal = 0;
            foreach (Coin coin in vm.StorageBin.Unload())
            {
                storagebintotal += coin.Value;
            }

            List<String> unsoldpoplist = new List<String>();
            foreach (PopCanRack poprack in vm.PopCanRacks)
            {
                foreach (PopCan pop in poprack.Unload())
                {
                    unsoldpoplist.Add(pop.Name);
                }
            }

            // CHECK_TEARDOWN

            int rackExpected = 1400;
            int storageExpected = 135;
            List<String> unsoldExpected = new List<String>();

            Assert.AreEqual(rackExpected, coinrackstotal);
            Assert.AreEqual(storageExpected, storagebintotal);
            CollectionAssert.AreEqual(unsoldExpected, unsoldpoplist);
        }
    }
}
