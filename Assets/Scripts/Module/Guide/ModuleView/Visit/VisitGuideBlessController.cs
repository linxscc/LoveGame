using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using Module.Guide.ModuleView.Visit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Module.Guide.Visit
{
    public class VisitGuideBlessController : Controller
    {

        public VisitGuideBlessView View;
        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
            }
        }
    }
}
