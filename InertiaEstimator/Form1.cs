using System.DirectoryServices;

namespace InertialEstimator
{
    public partial class Form1 : Form
    {
        bool mbIsFileChoosed = false;
        double mOriginX;
        double mOriginY;
        double mOriginZ;
        double mDensity;
        double mDiameter;

        public Form1()
        {
            InitializeComponent();
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.HelpButtonClicked += Form1_HelpButtonClicked;
        }

        private void Form1_HelpButtonClicked(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("- It is a software for estimating the Moment of " +
                "Inertia and Center of Mass of 3D printed parts.\r\n" +
                "- It parses the G-code file created by PrusaSlicer.\r\n" +
                "- The software is developed by Sunbin Kim. " +
                "If you have any questions, please contact me at einsbon@gmail.com"
            , "About this software.", MessageBoxButtons.OK);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!mbIsFileChoosed)
            {
                MessageBox.Show("Please choose a file first.");
                return;
            }
            if (!File.Exists(labelFilePath.Text))
            {
                MessageBox.Show("File not exist. " + labelFilePath.Text);
                return;
            }

            if (!double.TryParse(textBoxOriginX.Text, out mOriginX)
                || !double.TryParse(textBoxOriginY.Text, out mOriginY)
                || !double.TryParse(textBoxOriginZ.Text, out mOriginZ)
                || !double.TryParse(textBoxDensity.Text, out mDensity)
                || !double.TryParse(textBoxDiameter.Text, out mDiameter)
                || textBoxOriginX.Text == ""
                || textBoxOriginY.Text == ""
                || textBoxOriginZ.Text == ""
                || textBoxDensity.Text == ""
                || textBoxDiameter.Text == "")
            {
                MessageBox.Show("Enter numbers in the text boxes.");
                return;
            }


            string[] lines = System.IO.File.ReadAllLines(labelFilePath.Text);
            int lineLength = lines.Length;
            int lineCount = 0;


            double pastX = 0;
            double pastY = 0;
            double pastZ = 0;

            double presentX = 0;
            double presentY = 0;
            double presentZ = 0;

            bool bIsSupport = true;
            bool bIsExtrude = false;

            decimal wipeLength = 0;
            decimal presentExtrude = 0;
            decimal excludedExtrudeLength = 0;
            decimal totalExtrudeLength = 0;

            decimal ixx = 0;
            decimal iyy = 0;
            decimal izz = 0;

            decimal extrudeSumX = 0;
            decimal extrudeSumY = 0;
            decimal extrudeSumZ = 0;

            // gram of 1mm extruded filament
            // (extruded * massFactor) is gram of filament extruded
            double massFactor = (Math.PI * mDiameter * mDiameter * mDensity) / 4000.0;


            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = lineLength;
            progressBar1.Step = 1;


            foreach (string line in lines)
            {
                progressBar1.PerformStep();
                lineCount++;

                // Read comment to know the Freature Type of the print
                if (line.Length > 6 && line.Substring(0, 6) == ";TYPE:")
                {
                    string type = line.Substring(6);
                    switch (type)
                    {
                        case "Perimeter":
                            bIsSupport = false;
                            break;
                        case "External perimeter":
                            bIsSupport = false;
                            break;
                        case "Internal infill":
                            bIsSupport = false;
                            break;
                        case "Solid infill":
                            bIsSupport = false;
                            break;
                        case "Top solid infill":
                            bIsSupport = false;
                            break;
                        case "Bridge infill":
                            bIsSupport = false;
                            break;
                        case "Skirt/Brim":
                            bIsSupport = true;
                            break;
                        case "Support material":
                            bIsSupport = true;
                            break;
                        case "Support material interface":
                            bIsSupport = true;
                            break;
                        case "Custom":
                            bIsSupport = true;
                            break;
                        default:
                            break;
                    }
                }
                else if (line.Length > 2 && line.Substring(0, 3) == "G1 ")
                {
                    string[] parameters = line.Split(' ');

                    if (parameters[1][0] == 'X' || parameters[1][0] == 'Y' || parameters[1][0] == 'Z')
                    {
                        pastX = presentX;
                        pastY = presentY;
                        pastZ = presentZ;
                    }

                    bIsExtrude = false;

                    foreach (string parameter in parameters)
                    {
                        if (parameter[0] == 'X')
                        {
                            string xStr = parameter.Substring(1);
                            presentX = Convert.ToDouble(xStr);
                        }
                        else if (parameter[0] == 'Y')
                        {
                            string yStr = parameter.Substring(1);
                            presentY = Convert.ToDouble(yStr);
                        }
                        else if (parameter[0] == 'Z')
                        {
                            string zStr = parameter.Substring(1);
                            presentZ = Convert.ToDouble(zStr);
                        }
                        else if (parameter[0] == 'F')
                        {
                            //string F = word.Substring(1);
                            //textBoxF.Text = F[1];
                        }
                        else if (parameter[0] == 'E')
                        {
                            string extrudedStr = parameter.Substring(1);
                            bIsExtrude = true;
                            presentExtrude = Convert.ToDecimal(extrudedStr);
                            totalExtrudeLength += presentExtrude;
                        }
                        else if (parameter[0] == ';')
                        {
                            break;
                        }
                    }

                    if (!bIsExtrude)
                    {
                        continue;
                    }

                    if (presentExtrude < 0.0M)
                    {
                        wipeLength += presentExtrude;
                        continue;
                    }
                    else
                    {
                        if (wipeLength < 0.0m)
                        {
                            // ?????? wipe??? ????????????

                            if ((presentExtrude + wipeLength) > 0.001m)
                            {
                                // ?????? ?????? ???????????? wipe??? ?????? ???????????? ??????. 0.001??? ????????? ???.
                                presentExtrude = presentExtrude + wipeLength;
                                wipeLength = 0.0m;
                            }
                            else
                            {
                                // ?????? ?????? ???????????? wipe??? ???????????? ?????? ??????
                                wipeLength += presentExtrude;
                                continue;
                            }
                        }
                        else if (wipeLength > 0.0m)
                        {
                            presentExtrude = presentExtrude + wipeLength;
                            wipeLength = 0.0m;
                        }
                    }


                    if (bIsSupport)
                    {
                        continue;
                    }

                    // ????????? ????????? ????????? ?????? ?????? ?????? ?????? ??????.


                    //
                    // === ?????? ????????? ??????  ===

                    // 1. ?????? ?????? ????????? ?????? ???????????? ???????????? ??????????????? ?????????

                    // -- ??? ??? ???????????? ????????? ?????? 
                    double dX = presentX - pastX;
                    double dY = presentY - pastY;
                    double dZ = presentZ - pastZ;
                    double lXSquare = dY * dY + dZ * dZ; // mm^2
                    double lYSquare = dX * dX + dZ * dZ;
                    double lZSquare = dX * dX + dY * dY;

                    // -- ??????
                    double mass = (double)presentExtrude * massFactor; // gram

                    // -- ?????? ??????????????? ?????? ?????????. I = (1/12) * m * (l^2)
                    double icmX = (1.0 / 12.0) * mass * lXSquare; // g * mm^2
                    double icmY = (1.0 / 12.0) * mass * lYSquare;
                    double icmZ = (1.0 / 12.0) * mass * lZSquare;


                    // 2. ?????? ?????? ????????? ?????????

                    // -- ???????????? ???????????? ??? ????????? ?????? ??????
                    double centerX = ((presentX + pastX) / 2) - mOriginX; // mm
                    double centerY = ((presentY + pastY) / 2) - mOriginY;
                    double centerZ = ((presentZ + pastZ) / 2) - mOriginZ;

                    // -- ???????????? ??? ????????? ????????? ??????????????? ??????
                    double distFromXAxisToCenterSquare = centerY * centerY + centerZ * centerZ; // mm
                    double distFromYAxisToCenterSquare = centerX * centerX + centerZ * centerZ;
                    double distFromZAxisToCenterSquare = centerX * centerX + centerY * centerY;

                    // - ????????? ??????. I = Icm + m * (d^2) 
                    ixx += (decimal)(icmX + mass * distFromXAxisToCenterSquare); // g * mm^2
                    iyy += (decimal)(icmY + mass * distFromYAxisToCenterSquare);
                    izz += (decimal)(icmZ + mass * distFromZAxisToCenterSquare);


                    //
                    // === ?????? ?????? ????????? ?????? ??????  ===
                    extrudeSumX += (presentExtrude * (decimal)centerX); // ?????? ??????. ?????? ?????? ????????? ?????? (?????? ?????? * ?????? ??????) ??????.
                    extrudeSumY += (presentExtrude * (decimal)centerY);
                    extrudeSumZ += (presentExtrude * (decimal)centerZ);


                    //
                    // === ??? ????????? ??????????????? ????????? ?????? ?????? (????????? ?????? ??????) ===
                    excludedExtrudeLength += presentExtrude;
                }
            }


            // ?????? ?????? ?????? ??? ??????.


            // ??? ??????
            double totalMass = (double)totalExtrudeLength * massFactor; // gram
            double excludedMass = (double)excludedExtrudeLength * massFactor; // gram

            // ????????????
            double centerOfMassX = (double)(extrudeSumX / excludedExtrudeLength);
            double centerOfMassY = (double)(extrudeSumY / excludedExtrudeLength);
            double centerOfMassZ = (double)(extrudeSumZ / excludedExtrudeLength);

            // ????????????????????? ?????????????????? ????????? ?????? ??????
            // I = Icm + m * d^2
            // Icm = I - m * d^2
            double ixxCm = (double)ixx - excludedMass * (centerOfMassY * centerOfMassY + centerOfMassZ * centerOfMassZ);
            double iyyCm = (double)iyy - excludedMass * (centerOfMassX * centerOfMassX + centerOfMassZ * centerOfMassZ);
            double izzCm = (double)izz - excludedMass * (centerOfMassX * centerOfMassX + centerOfMassY * centerOfMassY);


            textBoxResult.Text =
                "Analyzed G-code file: " + labelFilePath.Text + "\r\n\r\n"
                + String.Format("Total filament used [mm]: {0:0.000}\r\n", totalExtrudeLength)
                + String.Format("Total filament used [g]: {0:0.000}\r\n\r\n", totalMass)
                + String.Format("Filament used on the part to be removed [mm]: {0:0.000}\r\n", excludedExtrudeLength)
                + String.Format("Mass excluding the part to be removed [g] {0:0.000}\r\n\r\n", excludedMass)
                + String.Format("Moment of inertia at ({0}, {1}, {2}) [g * mm^2]:\r\n",
                    mOriginX, mOriginY, mOriginZ)
                + String.Format(" ixx: {0:0.00000}\r\n", ixx)
                + String.Format(" iyy: {0:0.00000}\r\n", iyy)
                + String.Format(" izz: {0:0.00000}\r\n\r\n", izz)
                + String.Format("Moment of inertia at Center Of Mass ({0:0.000}, {1:0.000}, {2:0.000}) [g * mm^2]:\r\n",
                    centerOfMassX,
                    centerOfMassY,
                    centerOfMassZ)
                + String.Format(" ixx: {0:0.00000}\r\n", ixxCm)
                + String.Format(" iyy: {0:0.00000}\r\n", iyyCm)
                + String.Format(" izz: {0:0.00000}\r\n\r\n", izzCm)
                ;
            return;
        }

        private void buttonChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "GCode files (*.gcode)|*.gcode|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                labelFilePath.Text = openFileDialog.FileName;
                mbIsFileChoosed = true;
            }
        }
    }
}