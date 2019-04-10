from django.urls import path, re_path
from django.conf import settings
from django.conf.urls.static import static

from . import views

app_name = 'Forum'
urlpatterns = [
  path('register/', views.register, name='register'),
  path('about/', views.about, name='about'), 
  path('profile/', views.get_profile, name='get_profile'),
  path('message/', views.message, name='message'),
  path('inbox/', views.inbox, name='inbox'),
  path('announcement/', views.AnnouncementCreateView.as_view(), name='announcement'),
  path('msg_detail/', views.messagedetails, name='messagedetail'),
  path('topics/', views.TopicsView.as_view(), name='topics'),
  path('thread/<slug:pk>/', views.CommentThread.as_view(), name='thread'),
  re_path(r'^thread/(?P<slug>\w+)/comment$', views.create_comment, name='post_comment'),
  path('createthread/', views.ThreadCreateView.as_view(), name='create_thread'),
  path('friends/', views.FriendListView.as_view(), name='friends'),
  re_path(r'^friends/(?P<id>\w+)/', views.change_friend_status, name='friend_status'),
  re_path(r'^ajax/autocomplete/$', views.autocomplete, name='ajax_autocomplete'),
  re_path(r'^thread/upvote/(?P<id>\w+)$', views.upvote_thread, name='upvote'),
] + static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)
