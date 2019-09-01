using Asphalt;
using Asphalt.Api.Event;
using Asphalt.Api.Event.PlayerEvents;
using Asphalt.Service;
using Asphalt.Storeable;
using Eco.Core.Controller;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;
using Eco.Shared.Utils;

namespace ExtraStartStars
{
    [AsphaltPlugin(nameof(ExtraStartStars))]
    public class ExtraStartStars : IModKitPlugin
    {
        [Inject]
        [StorageLocation("config")]
        [DefaultValues(nameof(GetConfigValues))]
        public static IStorage Config { get; set; }

        [Inject]
        [StorageLocation("users")]
        public static IUserStorageCollection Users { get; set; }

        public ExtraStartStars()
        {
            EventManager.RegisterListener(this);
        }

        public string GetStatus()
        {
            return "Version 1.0";
        }

        public override string ToString()
        {
            return "Extra Start Stars";
        }

        [EventHandler]
        public void OnPlayerLogin(PlayerLoginEvent evt)
        {
            var user = evt.Player.User;

            IStorage storage = Users.GetStorage(user);

            if (storage.Get("GotStars") != null)
                return;

            storage.Set("GotStars", true);

            int toAdd = Config.GetInt("SpecialityPoints");

            user.SpecialtyPoints += toAdd;
            user.Changed(nameof(user.SpecialtyPoints));
            user.Changed(nameof(user.XP));
            Log.WriteLine(new LocString($"[{nameof(ExtraStartStars)}] User {user.Name} got {toAdd} and has now {user.SpecialtyPoints} SpecialtyPoints."));
        }

        public static KeyDefaultValue[] GetConfigValues()
        {
            return new KeyDefaultValue[]
            {
               new KeyDefaultValue("SpecialityPoints", 1),
            };
        }
    }
}
