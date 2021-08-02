﻿using FreshMvvm;
using InstagramClone.Models;
using InstagramClone.Services;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace InstagramClone.PageModels
{
    
    public class PostCommentsPageModel: FreshBasePageModel
    {
        public Command PostCommentCommand => new Command(PostComment);
        public ObservableCollection<Comment> PostCommentsCollection { get; set; }
        public int PostId { get; set; }
        public string UserImageUrl { get; set; }
        private string _CommentUserImageUrl;
        public string CommentUserImageUrl
        {
            set
            {
                _CommentUserImageUrl = value;
                RaisePropertyChanged();
            }
            get
            {
                return _CommentUserImageUrl;
            }
        }
        public string PostDescription { get; set; }
        public bool TaskInProcess { get; set; } = false;
        private string _UserLoggedImageUrl;
        public string UserLoggedImageUrl
        {
            set
            {
                _UserLoggedImageUrl = value;
                RaisePropertyChanged();
            }
            get
            {
                return _UserLoggedImageUrl;
            }
        }
        public string PostUserName { get; set; }
        private string _CommentText;
        public string CommentText
        {
            set
            {
                _CommentText = value;
                RaisePropertyChanged();
            }
            get
            {
                return _CommentText;
            }
        }
        public async void GetPostComments()
        {
            PostCommentsCollection.Clear();
            List<Comment> comments = await ApiService.GetPostComments(PostId);
            
            foreach (Comment comment in comments)
            {
                PostCommentsCollection.Add(comment);
            }
            //CommentUserImageUrl = PostCommentsCollection.
        }
        public PostCommentsPageModel()
        {
            PostCommentsCollection = new ObservableCollection<Comment>();
           
        }
        private async void GetUserLoggedInfo()
        {
            var userLoggedInfo = await ApiService.GetUserLoggedInfo();
            UserLoggedImageUrl = userLoggedInfo.FullImageUrl;

        }
        private async void PostComment(object obj)
        {
            if (TaskInProcess) { return; }
            TaskInProcess = true;
            var response = await ApiService.AddPostComment(CommentText, PostId);
            if (response)
            {
                //await CoreMethods.DisplayAlert("", "Comentario publicado correctamente", "Ok");
                GetPostComments();
                TaskInProcess = false;
                CommentText = "";
            }
            else
            {
                await CoreMethods.DisplayAlert("", "Error al comentar", "Ok");
                TaskInProcess = false;
            }
        }


        public  async override void Init(object initData)
        {
            GetUserLoggedInfo();
            //casting
            Post post = (Post)initData;
            UserImageUrl = post.FullUserImageUrl;
            PostId = post.Id;
            PostDescription = post.Description;
            PostUserName = post.UserName;
            List<Comment> comments = await ApiService.GetPostComments(post.Id);
            foreach(Comment comment in comments)
            {
                PostCommentsCollection.Add(comment);
            }
            base.Init(initData);
        }

    }
}
