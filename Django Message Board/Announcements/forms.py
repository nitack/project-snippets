from django import forms
from django.forms import ModelForm
from .models import *
from django.contrib.auth.forms import UserCreationForm
from django.contrib.auth.models import User


class AnnounceCreateForm(ModelForm):
  Title = forms.CharField(max_length=200, min_length=1, strip=True)
  Body = forms.CharField(
    max_length=1000,
    min_length=1,
    strip=True,
    widget=forms.Textarea,
  )


  class Meta:
    model = Announcement
    fields = ('Title', 'Body',)