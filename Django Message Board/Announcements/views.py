from django.shortcuts import render_to_response, render, redirect, reverse
from django.views import generic
from django.http import HttpResponseRedirect, HttpRequest, HttpResponse, HttpResponseBadRequest
from django.template import RequestContext
from django.contrib.auth import login, authenticate
from django.contrib.auth.models import User
from django import template
from django.contrib.auth.forms import UserCreationForm
from django.contrib.auth.decorators import login_required
from itertools import chain, groupby
from django.db import connection
from collections import namedtuple
import datetime
from .models import Comment, UserProfile, Topic, Thread, Message, FriendConnection
from django.db.models import Q
from functools import reduce
from django.views.generic.edit import FormView, CreateView
from django.views.decorators.http import require_http_methods
from django.http import JsonResponse
from .forms import ProfileForm, SignUpForm, CommentCreateForm, ThreadCreateForm, FriendRequestForm, AnnounceCreateForm
from .models import UserProfile, FriendConnection, Upvote, Announcement
# Create your views here.



# Create Announcement
class AnnouncementCreateView(CreateView):
  template_name = 'announcement.html'
  model = Announcement
  form_class = AnnounceCreateForm

  def form_valid(self, form):
    form = form.save(commit=False)
    form.Author = self.request.user
    today = datetime.date.today().strftime('%Y-%m-%d')
    form.DateAdded = today
    form.save()
    return HttpResponseRedirect("/".format(form.id))
