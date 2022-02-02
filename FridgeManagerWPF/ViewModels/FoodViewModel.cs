using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagerWPF.ViewModels
{
    public class FoodViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private Interfaces.IFood food;

        public FoodViewModel(Interfaces.IFood _food)
        {
            food = _food;
            IsChanged = false;
        }

        [Required(ErrorMessage = "Cannot be empty")]
        [Key]
        [Range(1, int.MaxValue, ErrorMessage = "Must be greater than 0")]
        public int FoodID
        {
            get => food.ID;
            set
            {
                food.ID = value;
                RaisePropertyChanged(nameof(FoodID));
            }
        }

        [Required(ErrorMessage = "Must have a name")]
        [MinLength(3, ErrorMessage = "Must be between 3 and 50 characters")]
        public string Name
        {
            get => food.Name;
            set
            {
                food.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        [Required]
        //[Range(1886, 2022)] TODO
        public DateTime ExpiryDate
        {
            get => food.ExpiryDate;
            set
            {
                food.ExpiryDate = value;
                RaisePropertyChanged(nameof(ExpiryDate));
            }
        }

        public Core.Storage Storage
        {
            get => food.Storage;
            set
            {
                food.Storage = value;
                RaisePropertyChanged(nameof(Storage));
            }
        }

        public Interfaces.IProducer Producer
        {
            get => food.Producer;
            set
            {
                food.Producer = value;
                RaisePropertyChanged(nameof(Producer));
            }
        }

        #region INotifyDataErrorInfo

        // nazwa wlasciwosci -> kolekcja bledow
        private Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        public bool HasErrors
        {
            get => _validationErrors.Count > 0;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName)) {
                return null;
            }
            return _validationErrors[propertyName];
        }

        #endregion

        
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            if (propertyName != nameof(HasErrors))
            {
                Validate();
            }
        }

        // temp
        /*
        private void validateFoodID()
        {
            List<string> errors = new List<string>();
            if (FoodID < 0)
            {
                errors.Add("ID must be a positive integer");
            }
            if (FoodID > 100)
            {
                errors.Add("Your fridge is full!");
            }

            if (_validationErrors.ContainsKey(nameof(FoodID)))
            {
                _validationErrors.Remove(nameof(FoodID));
            }
            if (errors.Count > 0)
            {
                _validationErrors[nameof(FoodID)] = errors;
            }
            RaiseErrorsChanged(nameof(FoodID));
        }
        */

        public void Validate()
        {
            var validationContext = new ValidationContext(this, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(this, validationContext, validationResults, true);

            // removing entries for which there are no longer errors
            foreach (var kv in _validationErrors.ToList())
            {
                if (validationResults.All(r => r.MemberNames.All(m => m != kv.Key)))
                {
                    _validationErrors.Remove(kv.Key);
                    RaiseErrorsChanged(kv.Key);
                }
            }

            var q = from result in validationResults
                    from member in result.MemberNames
                    group result by member into grup
                    select grup;

            foreach (var prop in q)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();
                // prop.

                if (_validationErrors.ContainsKey(prop.Key))
                {
                    _validationErrors.Remove(prop.Key);
                }
                _validationErrors.Add(prop.Key, messages);
                RaiseErrorsChanged(prop.Key);
            }
        }

        public void SaveItem()
        {
            BLC.BLC blc = BLC.BLC.GetBLC();
            blc.SaveItem(food);
            IsChanged = false;
        }

        private bool isChanged = false;

        public bool IsChanged
        {
            get => isChanged;
            set
            {
                isChanged = value;
                RaisePropertyChanged(nameof(IsChanged));
            }
        }
    }
}
