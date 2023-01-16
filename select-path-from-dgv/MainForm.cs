using System.ComponentModel;

namespace select_picture_from_dgv
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        internal BindingList<Card> Cards { get; } = new BindingList<Card>();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dataGridViewCards.AllowUserToAddRows = false;
            dataGridViewCards.RowHeadersVisible = false;
            dataGridViewCards.DataSource = Cards;
            dataGridViewCards.RowTemplate.Height = 100;

            #region F O R M A T    C O L U M N S
            Cards.Add(new Card()); // <- Auto generate columns
            dataGridViewCards.Columns["Name"].AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCards.Columns["FilePath"].AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewImageColumn imageColumn = (DataGridViewImageColumn) dataGridViewCards.Columns["Image"];
            imageColumn.AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.Width = 100;
            imageColumn.HeaderText = string.Empty;
            dataGridViewCards.Columns.Insert(2, new DataGridViewButtonColumn
            {
                Name = "Open",
                HeaderText = "Open",
            });
            Cards.Clear();
            #endregion F O R M A T    C O L U M N S

            // Add a few cards
            Cards.Add(new Card { Value = Value.Ten, Suit = Suit.Diamonds });
            Cards.Add(new Card { Value = Value.Jack, Suit = Suit.Clubs });
            Cards.Add(new Card { Value = Value.Queen, Suit = Suit.Hearts });
            Cards.Add(new Card { Value = Value.King, Suit = Suit.Spades });
            Cards.Add(new Card { Value = Value.Ace, Suit = Suit.Diamonds });

            dataGridViewCards.ClearSelection();
            dataGridViewCards.CellContentClick += onAnyCellContentClick;
        }

        private void onAnyCellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCards.Columns[e.ColumnIndex].Name.Equals("Open"))
            {
                var card = Cards[e.RowIndex];
                card.Image = card.GetCardImage();
            }
        }
    }
    enum Value { Joker, Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Back }
    enum Suit { Clubs, Diamonds, Hearts, Spades, }
    class Card : INotifyPropertyChanged
    {
        public Card() => Image = GetCardImage(faceUp: false);
        public string Name => $"{Value} of {Suit}";
        public string FilePath
        {
            get
            {
                switch (Value)
                {
                    case Value.Ace: return $"card{Suit}A.png";
                    case Value.Jack: return $"card{Suit}J.png";
                    case Value.Queen: return $"card{Suit}Q.png";
                    case Value.King: return $"card{Suit}K.png";
                    case Value.Joker: return $"cardJoker.png";
                    case Value.Back: return $"cardBack_green3.png";
                    default: return $"card{Suit}{(int)Value}.png";
                }
            }
        }

        [Browsable(false)]
        public Value Value { get; internal set; }

        [Browsable(false)]
        public Suit Suit { get; internal set; }
        Image? _image = null;
        public Image Image
        {
            get => _image;
            set
            {
                if (!Equals(_image, value))
                {
                    _image = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
                }
            }
        }

        public Image GetCardImage(bool faceUp = true)
        {
            string _imageBase = $"{GetType().Namespace}.Images.boardgamePack_v2.PNG.Cards";
            if (faceUp)
            {
                switch (Value)
                {
                    case Value.Ace: return localToImage($"{_imageBase}.card{Suit}A.png");
                    case Value.Jack: return localToImage($"{_imageBase}.card{Suit}J.png");
                    case Value.Queen: return localToImage($"{_imageBase}.card{Suit}Q.png");
                    case Value.King: return localToImage($"{_imageBase}.card{Suit}K.png");
                    case Value.Joker: return localToImage($"{_imageBase}.cardJoker.png");
                    case Value.Back:
                        return localToImage($"{_imageBase}.cardBack_green3.png");
                    default:
                        return localToImage($"{_imageBase}.card{Suit}{(int)Value}.png");
                }
            }
            else
            {
                return localToImage($"{_imageBase}.cardBack_green3.png");
            }
            Image localToImage(string resource)
            {
                using (var stream = GetType().Assembly.GetManifestResourceStream(resource)!)
                {
                    return new Bitmap(stream);
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}