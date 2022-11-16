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
                            // 아직 wipe가 남아있음

                            if ((presentExtrude + wipeLength) > 0.001m)
                            {
                                // 이번 출력 명령으로 wipe가 모두 사라지는 경우. 0.001은 오차로 봄.
                                presentExtrude = presentExtrude + wipeLength;
                                wipeLength = 0.0m;
                            }
                            else
                            {
                                // 이번 출력 명령으로 wipe가 사라지지 않는 경우
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

                    // 제거될 부분의 출력이 아닌 경우 아래 코드 수행.


                    //
                    // === 관성 모멘트 계산  ===

                    // 1. 직선 동작 출력을 가는 막대라고 가정하고 관성모멘트 구하기

                    // -- 각 축 기준으로 막대의 길이 
                    double dX = presentX - pastX;
                    double dY = presentY - pastY;
                    double dZ = presentZ - pastZ;
                    double lXSquare = dY * dY + dZ * dZ; // mm^2
                    double lYSquare = dX * dX + dZ * dZ;
                    double lZSquare = dX * dX + dY * dY;

                    // -- 질량
                    double mass = (double)presentExtrude * massFactor; // gram

                    // -- 막대 중심에서의 관성 모멘트. I = (1/12) * m * (l^2)
                    double icmX = (1.0 / 12.0) * mass * lXSquare; // g * mm^2
                    double icmY = (1.0 / 12.0) * mass * lYSquare;
                    double icmZ = (1.0 / 12.0) * mass * lZSquare;


                    // 2. 누적 관성 모멘트 구하기

                    // -- 기준점을 기준으로 한 막대의 중심 좌표
                    double centerX = ((presentX + pastX) / 2) - mOriginX; // mm
                    double centerY = ((presentY + pastY) / 2) - mOriginY;
                    double centerZ = ((presentZ + pastZ) / 2) - mOriginZ;

                    // -- 기준점의 각 축에서 막대의 중심까지의 거리
                    double distFromXAxisToCenterSquare = centerY * centerY + centerZ * centerZ; // mm
                    double distFromYAxisToCenterSquare = centerX * centerX + centerZ * centerZ;
                    double distFromZAxisToCenterSquare = centerX * centerX + centerY * centerY;

                    // - 평행축 정리. I = Icm + m * (d^2) 
                    ixx += (decimal)(icmX + mass * distFromXAxisToCenterSquare); // g * mm^2
                    iyy += (decimal)(icmY + mass * distFromYAxisToCenterSquare);
                    izz += (decimal)(icmZ + mass * distFromZAxisToCenterSquare);


                    //
                    // === 무게 중심 구하기 위한 누적  ===
                    extrudeSumX += (presentExtrude * (decimal)centerX); // 질량 아님. 무게 중심 구하기 위한 (출력 길이 * 출력 위치) 누적.
                    extrudeSumY += (presentExtrude * (decimal)centerY);
                    extrudeSumZ += (presentExtrude * (decimal)centerZ);


                    //
                    // === 총 질량과 무게중심을 구하기 위한 누적 (서포터 부분 제외) ===
                    excludedExtrudeLength += presentExtrude;
                }
            }


            // 누적 계산 완료 후 과정.


            // 총 질량
            double totalMass = (double)totalExtrudeLength * massFactor; // gram
            double excludedMass = (double)excludedExtrudeLength * massFactor; // gram

            // 무게중심
            double centerOfMassX = (double)(extrudeSumX / excludedExtrudeLength);
            double centerOfMassY = (double)(extrudeSumY / excludedExtrudeLength);
            double centerOfMassZ = (double)(extrudeSumZ / excludedExtrudeLength);

            // 무게중심에서의 관성모멘트를 구하기 위한 계산
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