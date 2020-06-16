using UnityEngine;

namespace Assets.Scripts.Framework.GalaSports.Interfaces
{
    public interface IPopup
    {
        GameObject Parent { get; set; }

        string ModuleName { get; set; }

        void OnShow(float delay);

        void Init();

        void Close();

    }
}
