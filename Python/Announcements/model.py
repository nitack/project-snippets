from django.db import models
from django.contrib.auth.models import User
from django.http import HttpResponseRedirect
from django.db.models.signals import post_save
from django.dispatch import receiver
from TTA import settings
import os


def user_directory_path(instance, filename):
  # TODO: Upload to media/user_<id>/avatar/<filename>
  #file will be uploaded to MEDIA_ROOT/user_<id>/<filename>
  return 'avatar/user_{0}/{1}'.format(instance.User.id, filename)


class Announcement(models.Model):
    Author = models.ForeignKey(User, null=True, on_delete=models.SET_NULL)
    DateAdded = models.DateField()
    Title = models.CharField(max_length=25)
    Body = models.CharField(max_length=1000)

    class Meta:
      get_latest_by = "DateAdded"