﻿using ColossalFramework.UI;


namespace RealisticPopulationRevisited
{
    /// <summary>
    /// Options panel for setting residential calculation options.
    /// </summary>
    internal class ResidentialPanel : PanelBase
    {
        // Array reference constants.
        private const int LowRes = 0;
        private const int HighRes = 1;
        private const int LowEcoRes = 2;
        private const int HighEcoRes = 3;
        private const int NumSubServices = 4;
        private const int NumLevels = 5;

        // Label constants.
        private string[] subServiceLables =
        {
            "Low res level ",
            "High res level ",
            "Low eco res lvl ",
            "High eco res lvl "
        };


        /// <summary>
        /// Adds residential options tab to tabstrip.
        /// </summary>
        /// <param name="tabStrip">Tab strip to add to</param>
        /// <param name="tabIndex">Index number of tab</param>
        public ResidentialPanel(UITabstrip tabStrip, int tabIndex)
        {
            // Add tab.
            UIPanel panel = PanelUtils.AddTab(tabStrip, "Residential", tabIndex);

            // Set residential flag.
            notResidential = false;

            // Initialise textfield array.
            SetupArrays(NumSubServices);

            for (int i = 0; i < NumSubServices; i ++)
            {
                areaFields[i] = new UITextField[NumLevels];
                floorFields[i] = new UITextField[NumLevels];
                powerFields[i] = new UITextField[NumLevels];
                waterFields[i] = new UITextField[NumLevels];
                sewageFields[i] = new UITextField[NumLevels];
                garbageFields[i] = new UITextField[NumLevels];
                incomeFields[i] = new UITextField[NumLevels];
            }

            // Headings.
            AddHeadings(panel);

            // Create residential per-person area textfields and labels.
            AddSubService(panel, subServiceLables[LowRes], true, LowRes);
            AddSubService(panel, subServiceLables[HighRes], true, HighRes);
            AddSubService(panel, subServiceLables[LowEcoRes], true, LowEcoRes);
            AddSubService(panel, subServiceLables[HighEcoRes], true, HighEcoRes);

            // Populate initial values.
            PopulateFields();

            // Add command buttons.
            AddButtons(panel);
        }


        /// <summary>
        /// Populates the text fields with information from the DataStore.
        /// </summary>
        protected override void PopulateFields()
        {
            // Populate each subservice.
            PopulateSubService(DataStore.residentialLow, LowRes);
            PopulateSubService(DataStore.residentialHigh, HighRes);
            PopulateSubService(DataStore.resEcoLow, LowEcoRes);
            PopulateSubService(DataStore.resEcoHigh, HighEcoRes);
        }


        /// <summary>
        /// Updates the DataStore with the information from the text fields.
        /// </summary>
        protected override void ApplyFields()
        {
            // Apply each subservice.
            ApplySubService(DataStore.residentialLow, LowRes);
            ApplySubService(DataStore.residentialHigh, HighRes);
            ApplySubService(DataStore.resEcoLow, LowEcoRes);
            ApplySubService(DataStore.resEcoHigh, HighEcoRes);

            // Clear cached values.
            DataStore.prefabHouseHolds.Clear();

            // Save new settings.
            XMLUtils.WriteToXML();

            // Refresh settings.
            PopulateFields();
        }


        /// <summary>
        /// Resets all textfields to mod default values.
        /// </summary>
        protected override void ResetToDefaults()
        {
            // Defaults copied from Datastore.
            int[][] residentialLow = { new int [] {2000, 50, -1, 0, -1,   -1, -1, -1, -1,    8, 20, 15, 11, 130,   0, 1,   -1, 35},
                                                 new int [] {2000, 50, -1, 0, -1,   -1, -1, -1, -1,    8, 21, 16, 10, 140,   0, 1,   -1, 30},
                                                 new int [] {2000, 50, -1, 0, -1,   -1, -1, -1, -1,    9, 22, 17, 10, 150,   0, 1,   -1, 25},
                                                 new int [] {2000, 50, -1, 0, -1,   -1, -1, -1, -1,    9, 24, 19,  9, 160,   0, 1,   -1, 20},
                                                 new int [] {2000, 50, -1, 0, -1,   -1, -1, -1, -1,   10, 26, 21,  9, 170,   0, 1,   -1, 15} };

            int[][] residentialHigh = { new int [] {140, 5, -1, 0, -1,   -1, -1, -1, -1,    7, 14, 11, 9, 90,   0, 5,   -1, 25},
                                                  new int [] {145, 5, -1, 0, -1,   -1, -1, -1, -1,    7, 15, 12, 8, 90,   0, 5,   -1, 20},
                                                  new int [] {150, 5, -1, 0, -1,   -1, -1, -1, -1,    8, 16, 13, 8, 90,   0, 5,   -1, 16},
                                                  new int [] {160, 5, -1, 0, -1,   -1, -1, -1, -1,    8, 17, 14, 7, 90,   0, 5,   -1, 12},
                                                  new int [] {170, 5, -1, 0, -1,   -1, -1, -1, -1,    9, 19, 16, 7, 90,   0, 5,   -1,  8} };

            int[][] resEcoLow = { new int [] {2000, 50, -1, 0, -1,   -1, -1, -1, -1,    6, 19, 15, 8,  91,   0, 1,   -1, 25 },
                                            new int [] {2000, 50, -1, 0, -1,   -1, -1, -1, -1,    6, 21, 17, 8,  98,   0, 1,   -1, 22},
                                            new int [] {2000, 50, -1, 0, -1,   -1, -1, -1, -1,    7, 23, 19, 7, 105,   0, 1,   -1, 18},
                                            new int [] {2000, 50, -1, 0, -1,   -1, -1, -1, -1,    8, 25, 21, 6, 112,   0, 1,   -1, 14},
                                            new int [] {2000, 50, -1, 0, -1,   -1, -1, -1, -1,    8, 28, 24, 6, 119,   0, 1,   -1, 10} };

            int[][] resEcoHigh = { new int [] {150, 5, -1, 0, -1,   -1, -1, -1, -1,    6, 14, 12, 7, 64,   0, 3,   -1, 20},
                                             new int [] {155, 5, -1, 0, -1,   -1, -1, -1, -1,    6, 16, 14, 6, 69,   0, 3,   -1, 15},
                                             new int [] {160, 5, -1, 0, -1,   -1, -1, -1, -1,    6, 18, 16, 6, 73,   0, 3,   -1, 12},
                                             new int [] {165, 5, -1, 0, -1,   -1, -1, -1, -1,    7, 20, 18, 5, 78,   0, 3,   -1,  9},
                                             new int [] {170, 5, -1, 0, -1,   -1, -1, -1, -1,    8, 22, 20, 5, 83,   0, 3,   -1,  6} };

            // Populate text fields with these.
            PopulateSubService(residentialLow, LowRes);
            PopulateSubService(residentialHigh, HighRes);
            PopulateSubService(resEcoLow, LowEcoRes);
            PopulateSubService(resEcoHigh, HighEcoRes);
        }
    }
}