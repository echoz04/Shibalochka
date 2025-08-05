using System;
using Sources.Runtime.Services.ProjectConfigLoader;
using UnityEngine;
using VContainer.Unity;

namespace Sources
{
    public class DiscordOverlayDisplayer : ITickable, IDisposable
    {
        private const long ApplicationId = 1396952100646158447;
        private const string LogoAdress = "logo_baobit";

        private Discord.Discord _discord;
        private IProjectConfigLoader _projectConfigLoader;

        public DiscordOverlayDisplayer(IProjectConfigLoader projectConfigLoader)
        {
            _projectConfigLoader = projectConfigLoader;
        }

        public void Initialize()
        {
            if (_projectConfigLoader.ProjectConfig.DiscordConfig.IsOverlayEnabled == false)
                return;

            _discord = new Discord.Discord(ApplicationId, (ulong)Discord.CreateFlags.NoRequireDiscord);

            SetActivity();
        }

        public void Tick()
        {
            _discord?.RunCallbacks();
        }

        public void Dispose()
        {
            _discord?.Dispose();
        }

        private void SetActivity()
        {
            var activityManager = _discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                State = _projectConfigLoader.ProjectConfig.DiscordConfig.OverlayTitle,
                Assets =
                {
                    LargeImage = LogoAdress,
                    LargeText = "Shibalochka"
                },
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                Debug.Log("Acitvity updated");
            });
        }
    }
}
