package com.peanut.discord.customview;
import android.content.Context;
import android.util.AttributeSet;

import androidx.appcompat.widget.AppCompatImageView;

import com.peanut.discord.R;
public class CopyImage extends AppCompatImageView {
    public CopyImage(Context context) {
        super(context);
    }
    public CopyImage(Context context, AttributeSet attrs) {
        super(context, attrs, R.attr.copyImageStyle);
    }
    public CopyImage(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
}
