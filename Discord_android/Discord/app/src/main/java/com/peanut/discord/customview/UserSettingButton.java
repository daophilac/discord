package com.peanut.discord.customview;

import android.content.Context;
import androidx.appcompat.widget.AppCompatImageButton;
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
