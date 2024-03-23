using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TiendaPC.Models;

namespace TiendaPC.ViewModels
{
    public class PedidosVM : INotifyPropertyChanged
    {
        private string _userName;
        private string _computerName;
        private string _address;
        private USUARIO _selectedUser;
        private COMPUTADORA _selectedComputer;

        public ObservableCollection<USUARIO> Usuarios { get; set; }
        public ObservableCollection<COMPUTADORA> Computadoras { get; set; }

        public USUARIO SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public COMPUTADORA SelectedComputer
        {
            get => _selectedComputer;
            set
            {
                _selectedComputer = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string ComputerName
        {
            get => _computerName;
            set
            {
                _computerName = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public PedidosVM()
        {
            Usuarios = new ObservableCollection<USUARIO>();
            Computadoras = new ObservableCollection<COMPUTADORA>();
            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var consumoServicios = new ConsumoServicios("");

                // Cargar Usuarios
                var usuarios = await consumoServicios.Get<List<USUARIO>>("https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/usuarios");
                Usuarios.Clear();
                foreach (var usuario in usuarios)
                {
                    Usuarios.Add(usuario);
                }

                // Cargar Computadoras
                var computadoras = await consumoServicios.Get<List<COMPUTADORA>>("https://apex.oracle.com/pls/apex/smeargle1628/api_tienda_pc/computadoras");
                Computadoras.Clear();
                foreach (var computadora in computadoras)
                {
                    Computadoras.Add(computadora);
                }
            }
            catch (Exception ex)
            {
                // Manejar el error según sea necesario
                Console.WriteLine("Error al cargar datos: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


