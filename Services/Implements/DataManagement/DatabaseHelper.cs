using SQLite;

using MAUI_IOT.Models;
using MAUI_IOT.Models.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;
#if ANDROID
using Android.OS;
using Android;
#endif


namespace MAUI_IOT.Services.Implements.DataManagement
{
    public class DatabaseHelper
    {
        private SQLiteAsyncConnection _conn;

        public DatabaseHelper(string fileName)
        {

            CheckAndRequestStoragePermission().Wait();

            string dbPath;

#if ANDROID
// Dùng đường dẫn ngoài app, ví dụ lưu trong thư mục Documents
            dbPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, fileName);
#elif WINDOWS
// Lưu file trong thư mục Downloads của Windows
            dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyDatabase.db3");
#else
            // Đối với các nền tảng khác như iOS hoặc Mac
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDatabase.db3");
#endif

            // Tạo kết nối đến SQLite
            _conn = new SQLiteAsyncConnection(dbPath);

            // Tạo bảng trong SQLite
            CreateTablesAsync();
        }


        private void CreateTablesAsync()
        {
            try
            {
                _conn.CreateTableAsync<Data>().Wait();
                _conn.CreateTableAsync<Experiment>().Wait();
                _conn.CreateTableAsync<ExperimentInfo>().Wait();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo bảng: {ex.Message}");
            }
        }
        public async Task AddAsync(Data data)
        {
            try
            {
                await _conn.InsertAsync(data);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
            }
        }

        public async Task AddAsync(Experiment data)
        {
            await _conn.InsertAsync(data);
        }

        public async Task AddAsync(ExperimentInfo data)
        {
            await _conn.InsertAsync(data);
        }

        public async Task<ObservableCollection<Data>> GetDataByExperimentId(int id)
        {
            var dataList = await _conn.Table<Data>()
                                      .Where(data => data.ExperimentInfoId == id)
                                      .ToListAsync();

            return new ObservableCollection<Data>(dataList);
        }

        public async Task<ObservableCollection<ExperimentInfo>> GetExperimentInfoById(int id)
        {
            var dataList =  await _conn.Table<ExperimentInfo>()
                                       .Where(data => data.ExperimentInfoId == id)
                                       .ToListAsync();
            return new ObservableCollection<ExperimentInfo>(dataList);
        }

        private async Task CheckAndRequestStoragePermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }

            if (status != PermissionStatus.Granted)
            {
                throw new UnauthorizedAccessException("Quyền truy cập lưu trữ chưa được cấp.");
            }
#if ANDROID
            if (Build.VERSION.SdkInt < BuildVersionCodes.R)
            {
                var activity = Platform.CurrentActivity ?? throw new InvalidOperationException("Activity không tồn tại.");

                // Yêu cầu quyền đọc ghi bộ nhớ ngoài
                activity.RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, 1);
            }
#endif
        }
    }
}
