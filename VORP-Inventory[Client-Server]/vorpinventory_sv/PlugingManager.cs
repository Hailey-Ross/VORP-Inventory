﻿using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VorpInventory.Diagnostics;
using VorpInventory.Models;
using VorpInventory.Scripts;

namespace VorpInventory
{
    public class PlugingManager : BaseScript
    {
        public static PlugingManager Instance { get; private set; }
        public static PlayerList PlayerList;

        public static Dictionary<int, PlayerInventory> PlayerInventories = new();

        public EventHandlerDictionary EventRegistry => EventHandlers;
        public ExportDictionary ExportRegistry => Exports;

        // private scripts
        Config _scriptConfig = new Config();

        public PlugingManager()
        {
            Logger.Info($"Init VORP Inventory");
            PlayerList = Players;

            Setup();

            Instance = this;
            Logger.Info($"VORP Inventory Loaded");
        }

        public void AttachTickHandler(Func<Task> task)
        {
            Tick += task;
        }

        public void DetachTickHandler(Func<Task> task)
        {
            Tick -= task;
        }

        async Task VendorReady()
        {
            while (!(GetResourceState("ghmattimysql") == "started"))
            {
                await Delay(500);
            }
        }

        async void Setup()
        {
            await VendorReady(); // wait till ghmattimysql resource has started

            Database.ItemDatabase.SetupItems();
            Database.ItemDatabase.SetupLoadouts();

            RegisterScript(_scriptConfig);

            AddEvents();
        }

        void AddEvents()
        {
            EventRegistry.Add("playerJoined", new Action<Player>(([FromSource] player) =>
            {

            }));

            EventRegistry.Add("playerDropped", new Action<Player>(([FromSource] player) =>
            {

            }));

            EventRegistry.Add("onResourceStart", new Action<string>(resourceName =>
            {
                if (resourceName != GetCurrentResourceName()) return;
            }));

            EventRegistry.Add("onResourceStop", new Action<string>(resourceName =>
            {
                if (resourceName != GetCurrentResourceName()) return;
                UnregisterScript(_scriptConfig);
            }));
        }
    }
}
