using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Life;
using Life.DB;
using Life.Network;
using Mirror;
using ModKit.Helper;
using ModKit.Helper.DiscordHelper;
using ModKit.Interfaces;

namespace Service100
{
    public class ServiceMetier : ModKit.ModKit
    {
        private readonly MyEvents _events;

        public ServiceMetier(IGameAPI api) : base(api)
        {
            PluginInformations = new PluginInformations(AssemblyHelper.GetName(), "1.0.0", "COLE100ST X Voici le discord si vous avez besoin d'aide : https://discord.gg/5B5xPpxVm");
            _events = new MyEvents(api);
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            _events.Init(Nova.server);
            ModKit.Internal.Logger.LogSuccess($"{PluginInformations.SourceName} v{PluginInformations.Version}", "initialisé");
        }

        public class MyEvents : ModKit.Helper.Events
        {
            public MyEvents(IGameAPI api) : base(api)
            {
            }

            public override void OnPlayerSpawnCharacter(Player player)
            {
                base.OnPlayerSpawnCharacter(player);

                int bizId = player.biz.Id;
                if (bizId > 0)
                {
                    player.serviceMetier = true;
                    player.Notify("INFORMATION", "Votre service métier a bien été activé automatiquemnt.", (Life.NotificationManager.Type)1);

                    List<Player> sameCompanyPlayers = Nova.server
                        .GetAllInGamePlayers()
                        .Where(p => p.biz.Id == bizId)
                        .ToList();
                    foreach (var otherPlayer in sameCompanyPlayers)
                    {
                        if (otherPlayer != player) 
                        {
                            otherPlayer.Notify("UN MEMBRE EST EN SERVICE", "Un collègue vient de se connecter.", NotificationManager.Type.Info);
                        }
                    }

                }
            }
        }
