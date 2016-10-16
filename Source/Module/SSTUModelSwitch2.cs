﻿using UnityEngine;
using System;

namespace SSTUTools
{
    public class SSTUModelSwitch2 : PartModule
    {

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "Variant"),
         UI_ChooseOption(suppressEditorShipModified = true)]
        public string currentModel = string.Empty;

        /// <summary>
        /// Index of the container in VolumeContainer that this model will influence the volume of
        /// </summary>
        [KSPField]
        public int containerIndex = 0;

        [Persistent]
        public string configNodeData = string.Empty;
        
        private PositionedModelData[] modelData;
        private PositionedModelData activeModel;
        private bool initialized = false;

        private void modelSelected(BaseField field, object obj)
        {

        }

        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            if (string.IsNullOrEmpty(configNodeData)) { configNodeData = node.ToString(); }
            initialize();
        }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            initialize();
            string[] names = SingleModelData.getModelNames(modelData);
            this.updateUIChooseOptionControl("currentModel", names, names, true, currentModel);
            Fields["currentModel"].uiControlEditor.onFieldChanged = modelSelected;
        }

        public override string GetInfo()
        {
            return "This part may have multiple model variants.  Check in the editor for more details.";
        }

        public void Start()
        {
            //TODO update resource volume for current setup
        }

        private void initialize()
        {
            if (initialized) { return; }
            initialized = true;
            ConfigNode node = SSTUConfigNodeUtils.parseConfigNode(configNodeData);
            ConfigNode[] nodes = node.GetNodes("MODEL");
            modelData = ModelData.parseModels<PositionedModelData>(nodes, m => new PositionedModelData(m));
            activeModel = Array.Find(modelData, m => m.name == currentModel);
        }
    }
}