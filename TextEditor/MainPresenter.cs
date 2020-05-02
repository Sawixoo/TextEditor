using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DfEditor;
using TextEditor.BL;

namespace TextEditor
{
    class MainPresenter
    {
        private readonly IMainForm _view;
        private readonly IfileManager _manager;
        private readonly IMessageService _messageService;
        private string _currentFilePath;

        public MainPresenter(IMainForm view, IfileManager manager, IMessageService service)
        {
            _view = view;
            _manager = manager;
            _messageService = service;


            _view.SetSymbolCount(0);
            _view.ContentChanged += new EventHandler(_view_ContentChanged);
            _view.FileOpenClick +=new EventHandler( _view_FileOpenClick);
            _view.FileSaveClick += new EventHandler (_view_FileSaveClick);
        
        }

        void _view_FileSaveClick(object sender, EventArgs e)
        {
            try
            {
                string content = _view.Content;
                _manager.SaveContent(content, _currentFilePath);
                _messageService.ShowMessage("file save");
            }
            catch(Exception ex)
            {
                _messageService.ShowError(ex.Message);
            }
        }

        void _view_FileOpenClick(object sender, EventArgs e)
        {
            try
            {
                string filePath = _view.FilePath;
                bool isExist = _manager.IsExist(filePath);
                if(!isExist)
                {
                    _messageService.ShowExclamation("Select file not found");
                    return;
                }
                _currentFilePath = filePath;
                string content = _manager.GetContent(filePath);
                int count = _manager.GetSymbolCount(content);
                _view.Content = content;
                _view.SetSymbolCount(count);
            }
            catch(Exception ex)
            {
                _messageService.ShowError(ex.Message);
            }
        }

        void _view_ContentChanged(object sender, EventArgs e)
        {
            string content = _view.Content;
            int count = _manager.GetSymbolCount(content);
            _view.SetSymbolCount(count);
        }
    }
}
