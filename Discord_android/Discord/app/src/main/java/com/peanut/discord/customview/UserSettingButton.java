package com.peanut.discord.customview;

import android.content.Context;
import android.support.v7.widget.AppCompatImageButton;
import android.util.AttributeSet;

import com.peanut.discord.R;

public class UserSettingButton extends AppCompatImageButton {
    public UserSettingButton(Context context) {
        super(context);
    }

    public UserSettingButton(Context context, AttributeSet attrs) {
        super(context, attrs, R.attr.userSettingButtonStyle);
    }

    public UserSettingButton(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
}
