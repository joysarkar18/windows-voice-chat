using System;
using System.Windows.Forms;
using Google.Cloud.Firestore;

namespace Voice_chat
{
    public partial class Form1 : Form
    {
        private FirestoreDb firestoreDb;


        public Form1()
        {
            InitializeComponent();
            textBox2.Text = Properties.Settings.Default.username;

            // Initialize Firestore
            InitializeFirestore();
        }

        private void InitializeFirestore()
        {
            // Path to your service account key JSON file
            string pathToServiceAccountKey = @"firebase-key.json"; // Update this path
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToServiceAccountKey);
            firestoreDb = FirestoreDb.Create("idea-usher-e59c4"); // Replace with your project ID
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Call the method to check username and hide or show button3
            UpdateButtonVisibility();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text1 = textBox1.Text;
            string text2 = textBox2.Text;

            // Display the text in a MessageBox or use it as needed
            MessageBox.Show($"TextBox1: {text1}\nTextBox2: {text2}");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // Additional functionality for button2 can be added here
          await  SaveUsernameToFirestore("joysarkar");
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string username = textBox2.Text.Trim();
            Properties.Settings.Default.username = username;
            Properties.Settings.Default.Save();
            MessageBox.Show("Username saved!");

            // Save to Firestore
            await SaveUsernameToFirestore(username);

            // After saving, update the button visibility
            UpdateButtonVisibility();
        }

        private async System.Threading.Tasks.Task SaveUsernameToFirestore(string username)
        {
            // Reference the 'users' collection
            CollectionReference usersCollection = firestoreDb.Collection("users");

            // Create a document with the username as the document ID
            DocumentReference userDoc = usersCollection.Document(username);
            await userDoc.SetAsync(new { Username = username }); // Save username in the document
        }

        private void UpdateButtonVisibility()
        {
            // Check if the username is empty or not
            if (string.IsNullOrEmpty(Properties.Settings.Default.username))
            {
                button3.Visible = true; // Show button3 if username is empty
                label2.Visible = true;
                textBox2.Visible = true;
                label3.Visible = false;
            }
            else
            {
                button3.Visible = false; // Hide button3 if username is not empty
                label2.Visible = false;
                textBox2.Visible = false;
                label3.Visible = true;
                label3.Text = "Welcome : " + Properties.Settings.Default.username;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Optional: Add functionality for label1 click
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Optional: Add functionality for label2 click
        }
    }
}
