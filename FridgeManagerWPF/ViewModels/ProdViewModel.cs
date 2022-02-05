using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FridgeManagerWPF.ViewModels
{
    public class ProdViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private Interfaces.IProducer producer;

        public ProdViewModel(Interfaces.IProducer _prod)
        {
            producer = _prod;
            IsChanged = false;
        }

        [Required(ErrorMessage = "Cannot be empty")]
        [Key]
        [Range(1, int.MaxValue, ErrorMessage = "Must be greater than 0")]
        public int ProdID
        {
            get => producer.ID;
            set
            {
                producer.ID = value;
                RaisePropertyChanged(nameof(ProdID));
            }
        }

        [Required(ErrorMessage = "Must have a name")]
        [MinLength(3, ErrorMessage = "Must be between 3 and 50 characters")]
        [MaxLength(50, ErrorMessage = "Must be between 3 and 50 characters")]
        public string Name
        {
            get => producer.Name;
            set
            {
                producer.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public Core.Region Residence
        {
            get => producer.Residence;
            set
            {
                producer.Residence = value;
                RaisePropertyChanged(nameof(Residence));
            }
        }

        #region INotifyDataErrorInfo

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
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
            {
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
            blc.SaveProducer(producer);
            IsChanged = false;
        }

        public void Remove()
        {
            BLC.BLC blc = BLC.BLC.GetBLC();
            blc.RemoveProducer(producer);
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
