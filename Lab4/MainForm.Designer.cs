//////////////////////////////////////////////////////////////////////////////////////////////////
// File Name: Tsp.Form.Designer.cs
//      Date: 06/01/2006
// Copyright (c) 2006 Michael LaLena. All rights reserved.
// Permission to use, copy, modify, and distribute this Program and its documentation,
//  if any, for any purpose and without fee is hereby granted, provided that:
//   (i) you not charge any fee for the Program, and the Program not be incorporated
//       by you in any software or code for which compensation is expected or received;
//   (ii) the copyright notice listed above appears in all copies;
//   (iii) both the copyright notice and this Agreement appear in all supporting documentation; and
//   (iv) the name of Michael LaLena or lalena.com not be used in advertising or publicity
//          pertaining to distribution of the Program without specific, written prior permission. 
///////////////////////////////////////////////////////////////////////////////////////////////////
namespace Lab4
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tourDiagram = new System.Windows.Forms.PictureBox();
            this.populationSizeTextBox = new System.Windows.Forms.TextBox();
            this.PopulationSizeLabel = new System.Windows.Forms.Label();
            this.lastIterationLabel = new System.Windows.Forms.Label();
            this.lastIterationValue = new System.Windows.Forms.Label();
            this.lastTourLabel = new System.Windows.Forms.Label();
            this.lastFitnessValue = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.maxGenerationLabel = new System.Windows.Forms.Label();
            this.maxGenerationTextBox = new System.Windows.Forms.TextBox();
            this.groupSizeLabel = new System.Windows.Forms.Label();
            this.groupSizeTextBox = new System.Windows.Forms.TextBox();
            this.clearCityListButton = new System.Windows.Forms.Button();
            this.mutationTextBox = new System.Windows.Forms.TextBox();
            this.mutationLabel = new System.Windows.Forms.Label();
            this.NumberCitiesLabel = new System.Windows.Forms.Label();
            this.NumberCitiesValue = new System.Windows.Forms.Label();
            this.NumberCloseCitiesTextBox = new System.Windows.Forms.TextBox();
            this.NumberCloseCitiesLabel = new System.Windows.Forms.Label();
            this.CloseCityOddsTextBox = new System.Windows.Forms.TextBox();
            this.CloseCityOddsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tourDiagram)).BeginInit();
            this.SuspendLayout();
            // 
            // tourDiagram
            // 
            this.tourDiagram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tourDiagram.BackColor = System.Drawing.Color.White;
            this.tourDiagram.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tourDiagram.Location = new System.Drawing.Point(12, 12);
            this.tourDiagram.Name = "tourDiagram";
            this.tourDiagram.Size = new System.Drawing.Size(408, 416);
            this.tourDiagram.TabIndex = 0;
            this.tourDiagram.TabStop = false;
            this.tourDiagram.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tourDiagram_MouseDown);
            // 
            // populationSizeTextBox
            // 
            this.populationSizeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.populationSizeTextBox.Location = new System.Drawing.Point(428, 40);
            this.populationSizeTextBox.Name = "populationSizeTextBox";
            this.populationSizeTextBox.Size = new System.Drawing.Size(116, 21);
            this.populationSizeTextBox.TabIndex = 1;
            this.populationSizeTextBox.Text = "1000";
            // 
            // PopulationSizeLabel
            // 
            this.PopulationSizeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PopulationSizeLabel.AutoSize = true;
            this.PopulationSizeLabel.Location = new System.Drawing.Point(428, 24);
            this.PopulationSizeLabel.Name = "PopulationSizeLabel";
            this.PopulationSizeLabel.Size = new System.Drawing.Size(116, 13);
            this.PopulationSizeLabel.TabIndex = 0;
            this.PopulationSizeLabel.Text = "Размер популяции";
            // 
            // lastIterationLabel
            // 
            this.lastIterationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lastIterationLabel.AutoSize = true;
            this.lastIterationLabel.Location = new System.Drawing.Point(12, 450);
            this.lastIterationLabel.Name = "lastIterationLabel";
            this.lastIterationLabel.Size = new System.Drawing.Size(128, 13);
            this.lastIterationLabel.TabIndex = 0;
            this.lastIterationLabel.Text = "Лучшее поколение: ";
            // 
            // lastIterationValue
            // 
            this.lastIterationValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lastIterationValue.Location = new System.Drawing.Point(138, 450);
            this.lastIterationValue.Name = "lastIterationValue";
            this.lastIterationValue.Size = new System.Drawing.Size(88, 13);
            this.lastIterationValue.TabIndex = 0;
            // 
            // lastTourLabel
            // 
            this.lastTourLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lastTourLabel.AutoSize = true;
            this.lastTourLabel.Location = new System.Drawing.Point(232, 451);
            this.lastTourLabel.Name = "lastTourLabel";
            this.lastTourLabel.Size = new System.Drawing.Size(82, 13);
            this.lastTourLabel.TabIndex = 0;
            this.lastTourLabel.Text = "Длина пути: ";
            // 
            // lastFitnessValue
            // 
            this.lastFitnessValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lastFitnessValue.Location = new System.Drawing.Point(349, 450);
            this.lastFitnessValue.Name = "lastFitnessValue";
            this.lastFitnessValue.Size = new System.Drawing.Size(85, 13);
            this.lastFitnessValue.TabIndex = 0;
            // 
            // StartButton
            // 
            this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StartButton.Location = new System.Drawing.Point(431, 385);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(121, 43);
            this.StartButton.TabIndex = 10;
            this.StartButton.Text = "Начать";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // maxGenerationLabel
            // 
            this.maxGenerationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxGenerationLabel.AutoSize = true;
            this.maxGenerationLabel.Location = new System.Drawing.Point(428, 136);
            this.maxGenerationLabel.Name = "maxGenerationLabel";
            this.maxGenerationLabel.Size = new System.Drawing.Size(131, 13);
            this.maxGenerationLabel.TabIndex = 0;
            this.maxGenerationLabel.Text = "Максимум поколений";
            // 
            // maxGenerationTextBox
            // 
            this.maxGenerationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxGenerationTextBox.Location = new System.Drawing.Point(428, 151);
            this.maxGenerationTextBox.Name = "maxGenerationTextBox";
            this.maxGenerationTextBox.Size = new System.Drawing.Size(116, 21);
            this.maxGenerationTextBox.TabIndex = 4;
            this.maxGenerationTextBox.Text = "150000";
            // 
            // groupSizeLabel
            // 
            this.groupSizeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSizeLabel.AutoSize = true;
            this.groupSizeLabel.Location = new System.Drawing.Point(428, 100);
            this.groupSizeLabel.Name = "groupSizeLabel";
            this.groupSizeLabel.Size = new System.Drawing.Size(94, 13);
            this.groupSizeLabel.TabIndex = 0;
            this.groupSizeLabel.Text = "Размер группы";
            // 
            // groupSizeTextBox
            // 
            this.groupSizeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSizeTextBox.Location = new System.Drawing.Point(428, 114);
            this.groupSizeTextBox.Name = "groupSizeTextBox";
            this.groupSizeTextBox.Size = new System.Drawing.Size(116, 21);
            this.groupSizeTextBox.TabIndex = 3;
            this.groupSizeTextBox.Text = "5";
            // 
            // clearCityListButton
            // 
            this.clearCityListButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearCityListButton.Location = new System.Drawing.Point(431, 333);
            this.clearCityListButton.Name = "clearCityListButton";
            this.clearCityListButton.Size = new System.Drawing.Size(121, 46);
            this.clearCityListButton.TabIndex = 9;
            this.clearCityListButton.Text = "Очистить";
            this.clearCityListButton.UseVisualStyleBackColor = true;
            this.clearCityListButton.Click += new System.EventHandler(this.clearCityListButton_Click);
            // 
            // mutationTextBox
            // 
            this.mutationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mutationTextBox.Location = new System.Drawing.Point(428, 78);
            this.mutationTextBox.Name = "mutationTextBox";
            this.mutationTextBox.Size = new System.Drawing.Size(116, 21);
            this.mutationTextBox.TabIndex = 2;
            this.mutationTextBox.Text = "15";
            // 
            // mutationLabel
            // 
            this.mutationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mutationLabel.AutoSize = true;
            this.mutationLabel.Location = new System.Drawing.Point(428, 64);
            this.mutationLabel.Name = "mutationLabel";
            this.mutationLabel.Size = new System.Drawing.Size(71, 13);
            this.mutationLabel.TabIndex = 10;
            this.mutationLabel.Text = "% мутации";
            // 
            // NumberCitiesLabel
            // 
            this.NumberCitiesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NumberCitiesLabel.Location = new System.Drawing.Point(434, 450);
            this.NumberCitiesLabel.Name = "NumberCitiesLabel";
            this.NumberCitiesLabel.Size = new System.Drawing.Size(64, 13);
            this.NumberCitiesLabel.TabIndex = 12;
            this.NumberCitiesLabel.Text = "Городов:";
            // 
            // NumberCitiesValue
            // 
            this.NumberCitiesValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NumberCitiesValue.Location = new System.Drawing.Point(496, 451);
            this.NumberCitiesValue.Name = "NumberCitiesValue";
            this.NumberCitiesValue.Size = new System.Drawing.Size(48, 13);
            this.NumberCitiesValue.TabIndex = 13;
            // 
            // NumberCloseCitiesTextBox
            // 
            this.NumberCloseCitiesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NumberCloseCitiesTextBox.Location = new System.Drawing.Point(428, 188);
            this.NumberCloseCitiesTextBox.Name = "NumberCloseCitiesTextBox";
            this.NumberCloseCitiesTextBox.Size = new System.Drawing.Size(116, 21);
            this.NumberCloseCitiesTextBox.TabIndex = 15;
            this.NumberCloseCitiesTextBox.Text = "4";
            // 
            // NumberCloseCitiesLabel
            // 
            this.NumberCloseCitiesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NumberCloseCitiesLabel.AutoSize = true;
            this.NumberCloseCitiesLabel.Location = new System.Drawing.Point(428, 172);
            this.NumberCloseCitiesLabel.Name = "NumberCloseCitiesLabel";
            this.NumberCloseCitiesLabel.Size = new System.Drawing.Size(120, 13);
            this.NumberCloseCitiesLabel.TabIndex = 14;
            this.NumberCloseCitiesLabel.Text = "Ближайшие города";
            // 
            // CloseCityOddsTextBox
            // 
            this.CloseCityOddsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseCityOddsTextBox.Location = new System.Drawing.Point(428, 228);
            this.CloseCityOddsTextBox.Name = "CloseCityOddsTextBox";
            this.CloseCityOddsTextBox.Size = new System.Drawing.Size(116, 21);
            this.CloseCityOddsTextBox.TabIndex = 18;
            this.CloseCityOddsTextBox.Text = "80";
            // 
            // CloseCityOddsLabel
            // 
            this.CloseCityOddsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseCityOddsLabel.AutoSize = true;
            this.CloseCityOddsLabel.Location = new System.Drawing.Point(428, 212);
            this.CloseCityOddsLabel.Name = "CloseCityOddsLabel";
            this.CloseCityOddsLabel.Size = new System.Drawing.Size(128, 13);
            this.CloseCityOddsLabel.TabIndex = 17;
            this.CloseCityOddsLabel.Text = "% ближайший город";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 473);
            this.Controls.Add(this.CloseCityOddsTextBox);
            this.Controls.Add(this.CloseCityOddsLabel);
            this.Controls.Add(this.NumberCloseCitiesTextBox);
            this.Controls.Add(this.NumberCloseCitiesLabel);
            this.Controls.Add(this.NumberCitiesValue);
            this.Controls.Add(this.NumberCitiesLabel);
            this.Controls.Add(this.mutationTextBox);
            this.Controls.Add(this.mutationLabel);
            this.Controls.Add(this.clearCityListButton);
            this.Controls.Add(this.groupSizeTextBox);
            this.Controls.Add(this.groupSizeLabel);
            this.Controls.Add(this.maxGenerationTextBox);
            this.Controls.Add(this.maxGenerationLabel);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.lastFitnessValue);
            this.Controls.Add(this.lastTourLabel);
            this.Controls.Add(this.lastIterationValue);
            this.Controls.Add(this.lastIterationLabel);
            this.Controls.Add(this.PopulationSizeLabel);
            this.Controls.Add(this.populationSizeTextBox);
            this.Controls.Add(this.tourDiagram);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Lab4";
            ((System.ComponentModel.ISupportInitialize)(this.tourDiagram)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox tourDiagram;
        private System.Windows.Forms.TextBox populationSizeTextBox;
        private System.Windows.Forms.Label PopulationSizeLabel;
        private System.Windows.Forms.Label lastIterationLabel;
        private System.Windows.Forms.Label lastIterationValue;
        private System.Windows.Forms.Label lastTourLabel;
        private System.Windows.Forms.Label lastFitnessValue;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label maxGenerationLabel;
        private System.Windows.Forms.TextBox maxGenerationTextBox;
        private System.Windows.Forms.Label groupSizeLabel;
        private System.Windows.Forms.TextBox groupSizeTextBox;
        private System.Windows.Forms.Button clearCityListButton;
        private System.Windows.Forms.TextBox mutationTextBox;
        private System.Windows.Forms.Label mutationLabel;
        private System.Windows.Forms.Label NumberCitiesLabel;
        private System.Windows.Forms.Label NumberCitiesValue;
        private System.Windows.Forms.TextBox NumberCloseCitiesTextBox;
        private System.Windows.Forms.Label NumberCloseCitiesLabel;
        private System.Windows.Forms.TextBox CloseCityOddsTextBox;
        private System.Windows.Forms.Label CloseCityOddsLabel;
    }
}

