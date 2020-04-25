﻿using UnityEngine;
using ColossalFramework.UI;


namespace RealisticPopulationRevisited
{
    /// <summary>
    /// Panel for editing and creating building settings.
    /// </summary>
    public class UIEditPanel : UIPanel
    {
        // Panel components
        private UITextField homeJobsCount;
        private UILabel homeJobLabel;
        private UIButton saveButton;
        private UIButton deleteButton;
        private UILabel messageLabel;

        // Currently selected building.
        private BuildingInfo currentSelection;
        

        /// <summary>
        /// Create the panel; called by Unity just before any of the Update methods is called for the first time.
        /// </summary>
        public override void Start()
        {
            const int marginPadding = 10;

            base.Start();

            // Generic setup.
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            backgroundSprite = "UnlockingPanel";
            autoLayout = false;
            autoLayoutDirection = LayoutDirection.Vertical;
            autoLayoutPadding.top = 5;
            autoLayoutPadding.right = 5;
            builtinKeyNavigation = true;
            clipChildren = true;

            // Panel title.
            UILabel title = this.AddUIComponent<UILabel>();
            title.relativePosition = new Vector3(0, 0);
            title.textAlignment = UIHorizontalAlignment.Center;
            title.text = "Custom settings";
            title.textScale = 1.2f;
            title.autoSize = false;
            title.width = this.width;
            title.height = 30;

            // Text field label.
            homeJobLabel = this.AddUIComponent<UILabel>();
            homeJobLabel.relativePosition = new Vector3(marginPadding, 40);
            homeJobLabel.textAlignment = UIHorizontalAlignment.Left;
            homeJobLabel.text = "Homes:";

            // Home or jobs count text field.
            homeJobsCount = UIUtils.CreateTextField(this, this.width - (marginPadding * 3) - homeJobLabel.width, 20);
            homeJobsCount.relativePosition = new Vector3(marginPadding + homeJobLabel.width + marginPadding, 40);

            // Save button.
            saveButton = UIUtils.CreateButton(this);
            saveButton.relativePosition = new Vector3(marginPadding, 70);
            saveButton.text = "Save";
            saveButton.Disable();

            saveButton.eventClick += (c, p) =>
            {
                // Hide message.
                messageLabel.isVisible = false;

                // Don't do anything with invalid entries.
                if (currentSelection == null || currentSelection.name == null)
                {
                    return;
                }

                // Read textfield if possible.
                if (int.TryParse(homeJobsCount.text, out int homesJobs))
                {
                    // Homes or jobs?  Add custom entry as appropriate.
                    if (currentSelection.GetService() == ItemClass.Service.Residential)
                    {
                        ExternalCalls.AddResidential(currentSelection.name, homesJobs);
                    }
                    else
                    {
                        ExternalCalls.AddWorker(currentSelection.name, homesJobs);
                    }

                    // Save to file.
                    ExternalCalls.Save();
                }
                else
                {
                    // TryParse couldn't parse the data; print warning message in red.
                    messageLabel.textColor = new Color32(255, 0, 0, 255);
                    messageLabel.text = "ERROR: invalid value";
                    messageLabel.isVisible = true;
                }
            };
            
            // Delete button.
            deleteButton = UIUtils.CreateButton(this);
            deleteButton.relativePosition = new Vector3(marginPadding, 110);
            deleteButton.text = "Delete";
            deleteButton.Disable();

            deleteButton.eventClick += (c, p) =>
            {
                // Hide message.
                messageLabel.isVisible = false;

                // Don't do anything with invalid entries.
                if (currentSelection == null || currentSelection.name == null)
                {
                    return;
                }

                // Homes or jobs?  Remove custom entry as appropriate.
                if (currentSelection.GetService() == ItemClass.Service.Residential)
                {
                    ExternalCalls.RemoveResidential(currentSelection.name);
                }
                else
                {
                    ExternalCalls.RemoveWorker(currentSelection.name);
                }

                // Save to file.
                ExternalCalls.Save();

                // Disable this button and clear input textfield.
                deleteButton.Disable();
                homeJobsCount.text = string.Empty;
            };

            // Message label (initially hidden).
            messageLabel = this.AddUIComponent<UILabel>();
            messageLabel.relativePosition = new Vector3(marginPadding, 160);
            messageLabel.textAlignment = UIHorizontalAlignment.Left;
            messageLabel.text = "No message to display";
            messageLabel.isVisible = false;
        }


        /// <summary>
        /// Called whenever the currently selected building is changed to update the panel display.
        /// </summary>
        /// <param name="building"></param>
        public void SelectionChanged(BuildingInfo building)
        {
            // Do nothing if the selection hasn't changed.
            if (currentSelection == building)
            {
                return;
            }

            // Hide message.
            messageLabel.isVisible = false;

            // Set current selecion.
            currentSelection = building;

            // Set text field to blank and disable buttons if no valid building is selected.
            if (building == null || building.name == null)
            {
                homeJobsCount.text = string.Empty;
                saveButton.Disable();
                deleteButton.Disable();
                return;
            }

            int homesJobs;

            if (building.GetService() == ItemClass.Service.Residential)
            {
                // See if a custom number of households applies to this building.
                homesJobs = ExternalCalls.GetResidential(building.name);
                homeJobLabel.text = "Homes:";
            }
            else
            {
                // Workplace building; see if a custom number of jobs applies to this building.
                homesJobs = ExternalCalls.GetWorker(building.name);
                homeJobLabel.text = "Jobs:";
            }

            // If no custom settings have been found (return value was zero), then blank the text field and disable the delete button.
            if (homesJobs == 0)
            {
                homeJobsCount.text = string.Empty;
                deleteButton.Disable();
            }
            else
            {
                // Valid custom settings found; display the result and enable the delete button.
                homeJobsCount.text = homesJobs.ToString();
                deleteButton.Enable();
            }

            // We've got a valid building, so enable the save button.
            saveButton.Enable();
        }
    }
}