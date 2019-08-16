package com.peanut.discord.customview;
import android.content.Context;
import android.util.AttributeSet;

import androidx.appcompat.widget.AppCompatImageView;

import com.peanut.discord.R;
public class UserImage extends AppCompatImageView {
    public UserImage(Context context) {
        super(context);
    }
    public UserImage(Context context, AttributeSet attrs) {
        super(context, attrs, R.attr.userImageStyle);
    }
    public UserImage(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
}
