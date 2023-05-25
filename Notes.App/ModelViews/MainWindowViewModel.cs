
using Notes.App.Commands;
using Notes.App.ModelViews.Base;
using System.Windows.Input;
using System;
using System.Windows.Threading;
using Notes.Db;
using Notes.App.ModelViews.Locators;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using Notes.Models.DbModels;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.IO;

namespace Notes.App.ModelViews
{
    internal sealed class MainWindowViewModel : ViewModelBase, IDisposable
    {
        #region fields
        #region dbContext : NotesDbContext - поле, доступное для чтения, предоставляющее доступ к экземпляру сущности, являющейся контекстом БД.
        /// <summary>
        /// Через себя позволяет обращаться к таблицам, сформированным на основе определенной модели, ссылочного типа.
        /// Представлены таблицы, экземпляром для типа DbSet, типизированные(в качестве типа для обобщения) с помощью модели, на основе которой
        /// и создается таблица внутри реляционный БД,SQL Server.
        /// </summary>
        public readonly NotesDbContext dbContext = DbContextLocator.notesDbContext;
        #endregion

        #endregion

        #region properties


        #region titleName : string - Титульное название главного окна.
        private string _titleName = "NoteApp";
        public string titleName
        {
            get => _titleName;
            set => Set(ref _titleName, value);
        }
        #endregion

        #region dateTimeNow : DateTime - Текущая: дата, время, минуты.
        private DateTime _dateTimeNow;
        public DateTime dateTimeNow
        {
            get => _dateTimeNow;
            set
            {
                Set(ref _dateTimeNow, value);
            }
        }
        #endregion


        #region recordsSortable : ICollectionView - свойство, предоставляющее доступ к экземпляру сущности, упакованному в ICollectionView тип.
        /// <summary>
        /// Позволяет заполучить экземпляр сущности коллекции, готовый к выводу в представлении, обработке с помощью среды создания GI.
        /// </summary>
        public ICollectionView recordsSortable => recordsSource.View;
        #endregion

        #region recordsSource : CollectionViewSource - свойство, предоставляющее ссылку на экземпляр сущности, являющейся составным объектом, хранящим ссылку на объект-коллекцию.
        private CollectionViewSource _recordsSource;
        public CollectionViewSource recordsSource => _recordsSource ??= CreateRecordListSource();
        private CollectionViewSource CreateRecordListSource()
        {
            CollectionViewSource collectionView = new CollectionViewSource();
            collectionView.Source = records;
            collectionView.Filter += OnRecordsFilterd;
            return collectionView;
        }
        #endregion

        #region records : ObservableCollection<Record> - Свойство, предоставляющее ссылку на экземпляр сущности, являющейся сложным объектом-коллекцией, обобщенной с помощью Record типа.
        public ObservableCollection<Record> records => GetRecords();
        private ObservableCollection<Record> GetRecords() => new ObservableCollection<Record>(dbContext.GetAllRecords());
        #endregion

        #region selectedRecord : Record - свойство, предоставляющее доступ к экземпляру для сущности Record. Записи, ссылочного типа.
        private Record _selectedRecord;
        public Record selectedRecord
        {
            get => _selectedRecord;
            set
            {
                Set(ref _selectedRecord, value);

                if (value != null)
                {
                    currentType = value.recordType;
                    currentTitleName = value.recordName;
                    typedRecordInfo = value.recordInfo;
                    choosenDate = value.recordCreationTime;
                }
            }
        }
        #endregion


        #region currentTitleName : String - текущее титульное название записи.
        private string _currentTitleName = "RecordTitle";
        public string currentTitleName
        {
            get => _currentTitleName;
            set => Set(ref _currentTitleName, value);
        }
        #endregion

        #region choosenDate : DateTime - Выбранная дата записи.
        private DateTime _choosenDate;
        public DateTime choosenDate
        {
            get => _choosenDate;
            set => Set(ref _choosenDate, value);
        }
        #endregion

        #region currentType : String - текущий тип записи.
        private string _currentType = "RecordType";
        public string currentType
        {
            get => _currentType;
            set => Set(ref _currentType, value);
        }
        #endregion


        #region typedTitleName : String - написанное титульное название записи.
        private string _typedTitleName;
        public string typedTitleName
        {
            get => _typedTitleName;
            set
            {
                Set(ref _typedTitleName, value);
                OnPropertyChanged(nameof(addRecordCommand));
            }
        }
        #endregion

        #region typedType : String - написанный тип записи.
        private string _typedType;
        public string typedType
        {
            get => _typedType;
            set
            {
                Set(ref _typedType, value);
                OnPropertyChanged(nameof(addRecordCommand));
            }
        }
        #endregion

        #region typedRecordInfo : String - написанная информация из заметки.
        private string _typedRecordInfo;
        public string typedRecordInfo
        {
            get => _typedRecordInfo;
            set
            {
                Set(ref _typedRecordInfo, value);
                OnPropertyChanged(nameof(addRecordCommand));
            }
        }
        #endregion


        #region recordsFilterText : string - строка, влияющая на процесс фильтрации выводимых экземпляров для сущности Record, в представлении программного решения.
        private string _recordsFilterText;
        public string recordsFilterText
        {
            get => _recordsFilterText;
            set
            {
                Set(ref _recordsFilterText, value);
                recordsSource.View.Refresh();
             }
        }
        #endregion
        #endregion

        public MainWindowViewModel()
        {
            startTimerCommand.Execute(null);
        }

        #region commands
        #region startTimerCommand - Процедура, осуществляющая начало слежки часов за временем, установленным на устройстве.
        private ICommand? _startTimerCommand;
        public ICommand startTimerCommand => _startTimerCommand ?? new LambdaCommand(o => true, OnStartTimerExecute);
        private void OnStartTimerExecute(object? param)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler((obj, ev) =>
            {
                dateTimeNow = DateTime.Now;
            });
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        #endregion

        #region removeRecordCommand
        private ICommand _removeRecordCommand;
        public ICommand removeRecordCommand => _removeRecordCommand ??= new LambdaCommand(CanRemoveRecordCommandExecuted,OnRemoveRecordCommandExecuteAsync);
        private bool CanRemoveRecordCommandExecuted(object? param) => selectedRecord != null;
        private async void OnRemoveRecordCommandExecuteAsync(object? param)
        {
            if (!(param is Record record))
                return;

            int index = records.IndexOf(record);

            dbContext.Remove<Record>(dbContext
                .GetAllRecords()
                .FirstOrDefault(r => r == record));

            await dbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            await Application
               .Current
               .Dispatcher
               .BeginInvoke(() =>
               {
                   OnPropertyChanged(nameof(records));
                   _recordsSource = CreateRecordListSource();
                   OnPropertyChanged(nameof(recordsSortable));
                   Dispose();
               });
        }
        #endregion

        #region addRecordCommand
        private ICommand _addRecordCommand;
        public ICommand addRecordCommand => _addRecordCommand ??= new LambdaCommand(CanAddRecordCommandExecuted, OnAddRecordCommandExecuteAsync);
        private bool CanAddRecordCommandExecuted(object? param) =>
            ( typedRecordInfo != null && typedRecordInfo.Length > 0 &&
            typedTitleName != null && typedTitleName.Length > 0 &&
            typedType != null && typedType.Length > 0);
        private async void OnAddRecordCommandExecuteAsync(object? param)
        {
            await dbContext
               .AddAsync(new Record(typedTitleName, typedType, typedRecordInfo, DateTime.Now))
               .ConfigureAwait(false);
            await dbContext
               .SaveChangesAsync()
               .ConfigureAwait(false);

            await Application
                .Current
                .Dispatcher
                .BeginInvoke(new Action(() =>
            {
                OnPropertyChanged(nameof(records));

                _recordsSource = CreateRecordListSource();
                OnPropertyChanged(nameof(recordsSortable));

                Dispose();
            }));

        }
        #endregion

        #region saveInTxtFileCommand
        private ICommand _saveInTxtFileCommand;
        public ICommand saveInTxtFileCommand => _saveInTxtFileCommand ??= new LambdaCommand(CanSaveInTxtFileCommandExecuted,OnSaveInTxtFileCommandExecute);
        private bool CanSaveInTxtFileCommandExecuted(object? param) => (!(string.IsNullOrEmpty(typedRecordInfo)));
        private void OnSaveInTxtFileCommandExecute(object? param)
        {
            SaveFileDialog saveTxtDialog = new SaveFileDialog();
            saveTxtDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            if (saveTxtDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveTxtDialog.FileName, typedRecordInfo + "\t[App was created by his majority, Edem Ablyakimov]");
            }
        }

        #endregion
        #endregion

        #region methods
        public void Dispose()
        {
            (typedRecordInfo, typedTitleName, typedType) = (null, null, null);
            (choosenDate, currentTitleName, currentType) = (DateTime.MinValue, "RecordTitle", "RecordType");
        }

        public void OnRecordsFilterd(object? initiator, FilterEventArgs e)
        {
            if (!(e.Item is Record record))
            {
                e.Accepted = false;
                return;
            }
            if (string.IsNullOrEmpty(recordsFilterText))
                return;

            if (record.recordName is null)
            {
                e.Accepted = false;
                return;
            }

            if (record.recordName.Contains(recordsFilterText, StringComparison.OrdinalIgnoreCase))
                return;

            e.Accepted = false ;
        }
        #endregion


    }
}
