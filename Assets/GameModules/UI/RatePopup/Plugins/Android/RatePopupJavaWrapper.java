package com.ratepopup.wrapper;

import android.os.AsyncTask;
import android.os.Build;
import android.util.Log;

import com.google.android.gms.tasks.Task;
import com.google.android.play.core.review.ReviewInfo;
import com.google.android.play.core.review.ReviewManager;
import com.google.android.play.core.review.ReviewManagerFactory;
import com.unity3d.player.UnityPlayer;

public class RatePopupJavaWrapper 
{
	    public static void showRatePopup() {
        if (android.os.Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {

            final ReviewManager reviewManager = ReviewManagerFactory.create(UnityPlayer.currentActivity.getApplicationContext());
            Task<ReviewInfo> request = reviewManager.requestReviewFlow();
            request.addOnCompleteListener(task -> {
                if (task.isSuccessful()) {
                    ReviewInfo reviewInfo = task.getResult();
                    Task<Void> flow = reviewManager.launchReviewFlow(UnityPlayer.currentActivity, reviewInfo);
                    flow.addOnCompleteListener(taskFlow -> {
                        Log.i("Log", "Did show review popup.");
                    });
                }
            });

        } else {

        }
    }
}
