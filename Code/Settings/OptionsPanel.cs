﻿using UnityEngine;
using ICities;
using ColossalFramework.UI;


namespace RealisticPopulationRevisited
{
    /// <summary>
    /// Class to handle the mod settings options panel.
    /// </summary>
    internal class OptionsPanel
    {
        // Parent UI panel reference.
        private static UIScrollablePanel optionsPanel;


        /// <summary>
        /// Options panel constructor.
        /// </summary>
        /// <param name="helper">UIHelperBase parent</param>
        internal OptionsPanel(UIHelperBase helper)
        {
            // Set up tab strip and containers.
            optionsPanel = ((UIHelper)helper).self as UIScrollablePanel;
            optionsPanel.autoLayout = false;

            UITabstrip tabStrip = optionsPanel.AddUIComponent<UITabstrip>();
            tabStrip.relativePosition = new Vector3(0, 0);
            tabStrip.size = new Vector2(744, 713);

            UITabContainer tabContainer = optionsPanel.AddUIComponent<UITabContainer>();
            tabContainer.relativePosition = new Vector3(0, 40);
            tabContainer.size = new Vector3(744, 713);
            tabStrip.tabPages = tabContainer;

            // Populate Datastore from configuration file.
            XMLUtilsWG.ReadFromXML();

            // Add tabs and panels.
            new ModOptionsPanel(tabStrip, 0);
            new ResidentialPanel(tabStrip, 1);
            new IndustrialPanel(tabStrip, 2);
            new CommercialPanel(tabStrip, 3);
            new OfficePanel(tabStrip, 4);

            // Start deactivated.
            optionsPanel.gameObject.SetActive(false);
        }


        /// <summary>
        /// Attaches an event hook to options panel visibility, to activate/deactivate our options panel as appropriate.
        /// Deactivating when not visible saves UI overhead and performance impacts, especially with so many UITextFields.
        /// </summary>
        public static void OptionsEventHook()
        {
            // Get options panel instance.
            UIPanel gameOptionsPanel = UIView.library.Get<UIPanel>("OptionsPanel");

            if (gameOptionsPanel == null)
            {
                Debugging.Message("couldn't find OptionsPanel");
            }
            else
            {
                // Simple event hook to enable/disable GameObject based on appropriate visibility.
                gameOptionsPanel.eventVisibilityChanged += (control, isVisible) =>
                {
                    optionsPanel.gameObject.SetActive(isVisible);
                };
            }
        }
    }
}