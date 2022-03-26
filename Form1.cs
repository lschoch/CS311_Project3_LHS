namespace CS311_Project3_LHS
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }// end frmMain

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            summarize();
        }// end btnCalculate_Click

        private void summarize()
        {
            float subTotal = 0.0F;
            float tax = 0.0F;
            float total = 0.0F;
            String crust = "";
            String toppings = "";

            // Make sure a crust has been selected
            var checkedButton = grpCrustType.Controls.OfType<RadioButton>()
                                      .FirstOrDefault(r => r.Checked);
            if (checkedButton != null) // A crust type has been selected - update crust variable.
                crust = checkedButton.Text;
            else // A crust type has not been selected - halt a request selection.
            {
                rtfOrderSummary.Clear();
                rtfOrderSummary.SelectionFont = new Font("Arial", 32);
                rtfOrderSummary.SelectionColor = Color.Red;
                rtfOrderSummary.SelectedText = "Please select a crust.";
                return;
            }

            List<Control> expensiveToppings = new List<Control>
                {ckbPepperoni, ckbSausage, ckbCanadianBacon, ckbSpicyItalianSausage};

            // Price the size.
            switch (cboSize.Text)
            {
                case "Small":
                    subTotal += 2.00F;
                    break;

                case "Medium":
                    subTotal += 5.00F;
                    break;

                case "Large":
                    subTotal += 10.00F;
                    break;

                case "X-Large":
                    subTotal += 15.00F;
                    break;

                case "Ginormous":
                    subTotal += 20.00F;
                    break;

                default: // A size has not been selected - halt and request selection.
                    rtfOrderSummary.Clear();
                    rtfOrderSummary.SelectionFont = new Font("Arial", 32);
                    rtfOrderSummary.SelectionColor = Color.Red;
                    rtfOrderSummary.SelectedText = "Please select a size.";
                    return;
            }// end switch

            // Price the toppings by iterating through the cells of the pnlToppings
            // TableLayoutPanel to see which items are checked. Charge accordingly.
            for (int row = 0; row < pnlToppings.RowCount; row++)
            {
                for (int col = 0; col < pnlToppings.ColumnCount; col++)
                {
                    // col = x coordinate, row = y coordinate
                    CheckBox? cb = pnlToppings.GetControlFromPosition(col, row) as CheckBox;
                    if (cb != null && cb.Checked)
                    {
                        
                        // Update the toppings string for the rtf output box.
                        toppings += cb.Text + "\n";
                        // Charge 2.00 if the topping is in the expensive list, 1.00 if not.
                        if (expensiveToppings.Contains(cb))
                            subTotal += 2.00F;
                        else
                            subTotal += 1.00F;
                    }
                }
            }

            // 6% sales tax.
            tax = subTotal * 6 / 100;
            total = subTotal + tax;

            txtSubTotal.Text = subTotal.ToString("C");
            txtTax.Text = tax.ToString("C");
            txtTotal.Text = total.ToString("C");

            // Select the correct article to use when referring to size.
            String article = "";
            if (cboSize.Text == "X-Large")
                article = "an";
            else
                article = "a";

            // Begin output to rich text box (rtfOrderSummary).
            rtfOrderSummary.Clear();
            rtfOrderSummary.SelectionFont = new Font("Segoe UI", 10);
            rtfOrderSummary.SelectionColor = Color.Black;
            rtfOrderSummary.SelectedText = $"You ordered {article} {cboSize.Text} pizza with {crust} "
                + $"crust and the following toppings:\n";
            
            // Update toppings variable for the case where no toppings are selected.
            if (toppings == "")
                toppings = "NONE\n";

            // Remove the last "\n" from toppings (to prevent a bulleted blank line
            // at the end of the toppings list in the rtf box).
            String trimmedToppings = toppings.Substring(0, toppings.Length-1);

            // Add trimmedtoppings variable to rich text box output with bullets. 
            rtfOrderSummary.SelectionBullet = true;
            rtfOrderSummary.SelectedText += trimmedToppings;
        }// end summarize

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exitProgram();
        }// end closeToolStripMenuItem

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.X)
                exitProgram();
        }// end frmMain_KeyDown

        private void exitProgram()
        {
            Environment.Exit(0);
        }// end exitProgram

    }// end class

}// end namespace